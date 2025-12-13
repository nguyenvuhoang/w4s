# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ solution
COPY . .

# Khôi phục package
WORKDIR /src/O24OpenAPI.Web.CMS
RUN dotnet restore "O24OpenAPI.Web.CMS.csproj"

# Build project với chế độ Release
RUN dotnet publish O24OpenAPI.Web.CMS.csproj -c Release -o /app/publish

# Stage 2: Tạo runtime container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy bản build từ stage build
COPY --from=build /app/publish .

# Mở cổng
EXPOSE 5000
EXPOSE 5001

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "O24OpenAPI.Web.CMS.dll"]
