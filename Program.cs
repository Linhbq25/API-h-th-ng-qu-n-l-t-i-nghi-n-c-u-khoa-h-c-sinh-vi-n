using Microsoft.OpenApi;
using Microsoft.OpenApi;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Helpers;
using quanlyNCKH.API.Repositories;
using quanlyNCKH.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddScoped<IDeTaiRepository, DeTaiRepository>();
builder.Services.AddScoped<IDeTaiService, DeTaiService>();

builder.Services.AddScoped<IDotDangKyRepository, DotDangKyRepository>();
builder.Services.AddScoped<IDotDangKyService, DotDangKyService>();

builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();

builder.Services.AddScoped<ISinhVienRepository, SinhVienRepository>();
builder.Services.AddScoped<ISinhVienService, SinhVienService>();

builder.Services.AddScoped<IGiangVienRepository, GiangVienRepository>();
builder.Services.AddScoped<IGiangVienService, GiangVienService>();

builder.Services.AddScoped<INhomSinhVienRepository, NhomSinhVienRepository>();
builder.Services.AddScoped<INhomSinhVienService, NhomSinhVienService>();

builder.Services.AddScoped<IThanhVienNhomRepository, ThanhVienNhomRepository>();
builder.Services.AddScoped<IThanhVienNhomService, ThanhVienNhomService>();

builder.Services.AddScoped<IDangKyDeTaiRepository, DangKyDeTaiRepository>();
builder.Services.AddScoped<IDangKyDeTaiService, DangKyDeTaiService>();

builder.Services.AddScoped<IBaiNopRepository, BaiNopRepository>();
builder.Services.AddScoped<IBaiNopService, BaiNopService>();

builder.Services.AddScoped<ITieuChiDanhGiaRepository, TieuChiDanhGiaRepository>();
builder.Services.AddScoped<ITieuChiDanhGiaService, TieuChiDanhGiaService>();

builder.Services.AddScoped<IDanhGiaRepository, DanhGiaRepository>();
builder.Services.AddScoped<IDanhGiaService, DanhGiaService>();

builder.Services.AddScoped<IThongKeRepository, ThongKeRepository>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();

 

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();