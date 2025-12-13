

function decodeNumberToString(number, length = 8) {

    let hexString = number.toString(16);
    if (hexString.length % 2 !== 0) {
        hexString = '0' + hexString;
    }

    let charArray = [];
    for (let i = 0; i < hexString.length; i += 2) {
        let byte = parseInt(hexString.substr(i, 2), 16);
        charArray.push(String.fromCharCode(byte));
    }
    if (charArray.length < length) {
        charArray = arr_rjust(charArray, length);
    } else if (charArray.length > length) {

        charArray = charArray.slice(-length);
    }

    return charArray.join('');
}


/**
 * Pads an array from the left with the specified character to the specified length.
 * @param {Array} array - The array to pad.
 * @param {number} length - The desired length of the output array.
 * @param {string} [padChar=' '] - The character to pad the array with.
 * @returns {Array} - The padded array.
 */
function arr_rjust(t, n) {
    var u = t;
    if (n <= u.length) return u.splice(u.length - n);
    for (var o = n - u.length, l = 0; l < o; l += 1) u.unshift(String.fromCharCode(0));
    return u;
}

function generatePrivateKey(input ){
    const decode = new EncoderDecoder({
        dataBits: 8,
        codeBits: 5,
        keyString: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ234567',
        pad: '=',
    });
    return  decode.encode(input);
}

function generateOTPWithTransId(transId,privateKey,currentTime) {
  
    const hmac = new X.jsSHA("SHA-1", "BYTES");
    const decode = new EncoderDecoder({
        dataBits: 8,
        codeBits: 5,
        keyString: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ234567',
        pad: '='
    });
    const timeString = currentTime.toString();
    const combinedData = transId + timeString;
    hmac.setHMACKey(decode.decode(privateKey.toUpperCase()), 'BYTES');
    hmac.update(decodeNumberToString(combinedData));

    const hmacResult = hmac.getHMAC('BYTES').split('');
    const offset = hmacResult[hmacResult.length - 1].charCodeAt() & 0xf;

    const binary = (
        ((hmacResult[offset].charCodeAt() & 0x7f) << 24) |
        ((hmacResult[offset + 1].charCodeAt() & 0xff) << 16) |
        ((hmacResult[offset + 2].charCodeAt() & 0xff) << 8) |
        (hmacResult[offset + 3].charCodeAt() & 0xff)
    );

    let otp = (binary % Math.pow(10, 8)).toString();

    return otp;
}

// Function để tạo mã TOTP
function generateOTPNonTransId(secretKey, interval = 300) {  // Interval là 300 giây = 5 phút
    const epoch = Math.round(new Date().getTime() / 1000.0);
    const timeStep = Math.floor(epoch / interval);

    let timeHex = timeStep.toString(16).padStart(16, '0');
    const shaObj = new X.jsSHA("SHA-1", "BYTES");
    shaObj.setHMACKey(secretKey, "TEXT");
    shaObj.update(timeHex);

    const hmac = shaObj.getHMAC("HEX");

    const offset = parseInt(hmac.substring(hmac.length - 1), 16);

    const binaryCode = (parseInt(hmac.substr(offset * 2, 8), 16) & 0x7fffffff).toString();

    const otp = binaryCode.substr(binaryCode.length - 6);

    return otp;
}

// Hàm để kiểm tra mã TOTP
function verifyOTPNonTransId(secretKey, userInput, interval = 300, allowedDrift = 1) {
    // Lấy thời gian hiện tại
    const epoch = Math.round(new Date().getTime() / 1000.0);
    const timeStep = Math.floor(epoch / interval);

    // Kiểm tra mã TOTP sinh ra trong các khoảng thời gian lệch (chênh lệch allowedDrift bước)
    for (let i = -allowedDrift; i <= allowedDrift; i++) {
        const currentStep = timeStep + i;

        // Chuyển đổi currentStep thành hex và pad nó
        let timeHex = currentStep.toString(16).padStart(16, '0');
        const shaObj = new X.jsSHA("SHA-1", "BYTES");
        shaObj.setHMACKey(secretKey, "TEXT");
        shaObj.update(timeHex);

        const hmac = shaObj.getHMAC("HEX");

        const offset = parseInt(hmac.substring(hmac.length - 1), 16);

        const binaryCode = (parseInt(hmac.substr(offset * 2, 8), 16) & 0x7fffffff).toString();

        const otp = binaryCode.substr(binaryCode.length - 6);

        // So sánh mã do người dùng nhập vào với mã sinh ra
        if (otp === userInput) {
            return true;  // Mã hợp lệ
        }
    }

    return false; // Mã không hợp lệ
}

class EncoderDecoder {

    constructor(options) {
        this.padChar = options.pad || '';  // Character to use for padding
        this.dataBits = options.dataBits;  // Number of bits for the data
        this.codeBits = options.codeBits;  // Number of bits for the encoded characters
        this.keyString = options.keyString;  // The key string for encoding/decoding
        this.useArrayData = options.arrayData;  // Boolean indicating if array data should be used

        this.lookupTable = [];
        this.maxBits = 0;
        this.blockSize = 0;

        this.initialize();
    }

    // Initialize function, which sets up necessary variables and lookup tables
    initialize() {
        const maxBitLength = Math.max(this.dataBits, this.codeBits);
        let value = 0;

        for (let i = 0; i < maxBitLength; i++) {
            this.lookupTable.push(value);
            value += value + 1;
        }

        this.maxBits = value;
        this.blockSize = this.dataBits / this.gcd(this.dataBits, this.codeBits);
    }

    // Helper function to calculate the greatest common divisor (GCD)
    gcd(a, b) {
        while (b !== 0) {
            const temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // Core function to handle encoding and decoding
    process(input, inputBits, outputBits, isDecoding) {
        let buffer = 0;
        let bufferBits = 0;
        const resultArray = [];

        const addToResult = (value) => {
            if (isDecoding) {
                if (this.useArrayData) {
                    resultArray.push(value);
                } else {
                    resultArray.push(String.fromCharCode(value));
                }
            } else {
                resultArray.push(this.keyString.charAt(value));
            }
        };

        for (let i = 0; i < input.length; i++) {
            bufferBits += inputBits;
            let current;

            if (isDecoding) {
                current = this.keyString.indexOf(input.charAt(i));
                if (input.charAt(i) === this.padChar) break;
                if (current < 0) throw new Error(`The character "${input.charAt(i)}" is not a member of ${this.keyString}`);
            } else {
                current = this.useArrayData ? input[i] : input.charCodeAt(i);
                if ((current | this.maxBits) !== this.maxBits) throw new Error(`${current} is outside the range 0-${this.maxBits}`);
            }

            buffer = (buffer << inputBits) | current;

            while (bufferBits >= outputBits) {
                addToResult(buffer >> (bufferBits -= outputBits));
                buffer &= this.lookupTable[bufferBits];
            }
        }

        if (!isDecoding && bufferBits > 0) {
            addToResult(buffer << (outputBits - bufferBits));
            while (resultArray.length % this.blockSize !== 0) {
                resultArray.push(this.padChar);
            }
        }

        return this.useArrayData && isDecoding ? resultArray : resultArray.join('');
    }

    // Encode function
    encode(input) {
        return this.process(input, this.dataBits, this.codeBits, false);
    }

    // Decode function
    decode(input) {
        return this.process(input, this.codeBits, this.dataBits, true);
    }
}

