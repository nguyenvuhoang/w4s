CREATE OR REPLACE FUNCTION o24cth.get_system_code_caption (
    p_codeid   IN NVARCHAR2,
    p_codename IN NVARCHAR2,
    p_codegrp  IN NVARCHAR2
) RETURN NVARCHAR2
IS
    v_caption NVARCHAR2(4000);
BEGIN
    SELECT caption
      INTO v_caption
      FROM c_codelist
     WHERE codegroup = p_codegrp
       AND codename = p_codename
       AND codeid   = p_codeid
     FETCH FIRST 1 ROWS ONLY;

    RETURN v_caption;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN p_codeid;
END;
