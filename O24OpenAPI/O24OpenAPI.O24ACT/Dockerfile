# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ solution
COPY . .

# Khôi phục package
WORKDIR /src/O24OpenAPI.O24ACT
RUN dotnet restore "O24OpenAPI.O24ACT.csproj"

# Build project với chế độ Release
RUN dotnet publish O24OpenAPI.O24ACT.csproj -c Release -o /app/publish

# Stage 2: Tạo runtime container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy bản build từ stage build
COPY --from=build /app/publish .

# Mở cổng
EXPOSE 105
EXPOSE 109

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "O24OpenAPI.O24ACT.dll"]
