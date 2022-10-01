using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class TN230Context : DbContext
    {
        public TN230Context()
        {
        }

        public TN230Context(DbContextOptions<TN230Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AnhN> AnhNs { get; set; }
        public virtual DbSet<BaiViet> BaiViets { get; set; }
        public virtual DbSet<ChiTietNsDd> ChiTietNsDds { get; set; }
        public virtual DbSet<DonDat> DonDats { get; set; }
        public virtual DbSet<DonViTinh> DonViTinhs { get; set; }
        public virtual DbSet<GianHang> GianHangs { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<DonXinDT> DonXinDTs { get; set; }
        public virtual DbSet<LoaiNongSan> LoaiNongSans { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<NongSan> NongSans { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<Trang_Thai> TrangThais { get; set; }
        public object NongSan { get; internal set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-VFOQC2P\\SQL_SERVER;Database=TN230_V1;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Vietnamese_CI_AS");

            modelBuilder.Entity<AnhN>(entity =>
            {
                entity.HasKey(e => e.MaAnh);

                entity.ToTable("Anh_NS");

                entity.Property(e => e.MaAnh).HasColumnName("Ma_Anh");

                entity.Property(e => e.LinkAnh)
                    .HasMaxLength(50)
                    .HasColumnName("Link_Anh");

                entity.Property(e => e.MaNongSan).HasColumnName("Ma_Nong_San");

                entity.Property(e => e.MoTa)
                    .HasMaxLength(150)
                    .HasColumnName("Mo_Ta");

                entity.HasOne(d => d.MaNongSanNavigation)
                    .WithMany(p => p.AnhNs)
                    .HasForeignKey(d => d.MaNongSan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anh_NS_Nong_San");
            });
            modelBuilder.Entity<GioHang>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.MaTaiKhoan);

                entity.HasKey(e => e.MaNongSan);

                entity.ToTable("GioHang");

                entity.Property(e => e.MaTaiKhoan).HasColumnName("Ma_Tai_Khoan");

                entity.Property(e => e.MaNongSan).HasColumnName("Ma_Nong_San");

                entity.Property(e => e.SL).HasColumnName("SL");

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GioHang_Tai_Khoan");

                entity.HasOne(d => d.MaNongSanNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaNongSan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GioHang_Nong_San");
            });

            modelBuilder.Entity<BaiViet>(entity =>
            {
                entity.HasKey(e => e.MaTinTuc);

                entity.ToTable("Bai_Viet");

                entity.Property(e => e.MaTinTuc).HasColumnName("Ma_Tin_Tuc");

                entity.Property(e => e.LinkAnh)
                    .HasMaxLength(50)
                    .HasColumnName("Link_Anh");

                entity.Property(e => e.MaNhanVien).HasColumnName("Ma_Nhan_Vien");

                entity.Property(e => e.NoiDung)
                    .HasMaxLength(50)
                    .HasColumnName("Noi_Dung");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.BaiViets)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK_Bai_Viet_Nhan_Vien");
            });

            modelBuilder.Entity<ChiTietNsDd>(entity =>
            {
                entity.HasKey(e => e.MaDonDat);

                entity.HasKey(e => e.MaNongSan);

                entity.ToTable("Chi_Tiet_NS_DD");

                entity.Property(e => e.MaDonDat).HasColumnName("Ma_Don_Dat");

                entity.Property(e => e.MaNongSan).HasColumnName("Ma_Nong_San");

                entity.Property(e => e.SoLuongDat).HasColumnName("So_Luong_Dat");

                entity.HasOne(d => d.MaDonDatNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaDonDat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chi_Tiet_NS_DD_Don_Dat");

                entity.HasOne(d => d.MaNongSanNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaNongSan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chi_Tiet_NS_DD_Nong_San");
            });
           
            modelBuilder.Entity<DonDat>(entity =>
            {
                entity.HasKey(e => e.MaDonDat);

                entity.ToTable("Don_Dat");

                entity.Property(e => e.MaDonDat).HasColumnName("Ma_Don_Dat");

                entity.Property(e => e.MaNguoiDung).HasColumnName("Ma_Nguoi_Dung");

                entity.Property(e => e.NgayDatHang)
                    .HasMaxLength(50)
                    .HasColumnName("Ngay_Dat_Hang");

                entity.Property(e => e.Ma_Trang_Thai)
                    .HasMaxLength(50)
                    .HasColumnName("Ma_Trang_Thai");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DonDats)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Don_Dat_Nguoi_Dung");
                entity.HasOne(d => d.MaTrangThaiNavigation)
                    .WithMany(p => p.DonDats)
                    .HasForeignKey(d => d.Ma_Trang_Thai)
                    .HasConstraintName("FK_Don_Dat_Trang_Thai");
            });

            modelBuilder.Entity<DonViTinh>(entity =>
            {
                entity.HasKey(e => e.MaDonViTinh);

                entity.ToTable("Don_Vi_Tinh");

                entity.Property(e => e.MaDonViTinh).HasColumnName("Ma_Don_Vi_Tinh");

                entity.Property(e => e.TenDonViTinh)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Don_vi_Tinh");
            });

            modelBuilder.Entity<GianHang>(entity =>
            {
                entity.HasKey(e => e.MaGianHang);

                entity.ToTable("Gian_Hang");

                entity.Property(e => e.MaGianHang).HasColumnName("Ma_Gian_Hang");

                entity.Property(e => e.MaNguoiDung).HasColumnName("Ma_Nguoi_Dung");

                entity.Property(e => e.TenGianHang)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Gian_Hang");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.GianHangs)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Gian_Hang_Nguoi_Dung");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHoaDon);

                entity.ToTable("Hoa_Don");

                entity.Property(e => e.MaHoaDon).HasColumnName("Ma_Hoa_Don");

                entity.Property(e => e.MaDonDat).HasColumnName("Ma_Don_Dat");

                entity.Property(e => e.NgayGiaoHang)
                    .HasMaxLength(50)
                    .HasColumnName("Ngay_Giao_Hang");

                entity.HasOne(d => d.MaDonDatNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaDonDat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hoa_Don_Don_Dat");
            });
            modelBuilder.Entity<DonXinDT>(entity =>
            {
                entity.HasKey(e => e.MaDoiTac);

                entity.ToTable("DonXinDT");

                entity.Property(e => e.MaDoiTac).HasColumnName("Ma_Doi_Tac");

                entity.Property(e => e.MaNguoiDung).HasColumnName("Ma_Nguoi_Dung");

                entity.Property(e => e.NgayDuyet)
                    .HasMaxLength(50)
                    .HasColumnName("Ngay_Duyet");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DonXinDTs)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DonXinDT_Nguoi_Dung");
            });

            modelBuilder.Entity<LoaiNongSan>(entity =>
            {
                entity.HasKey(e => e.MaLoaiNongSan);

                entity.ToTable("Loai_Nong_San");

                entity.Property(e => e.MaLoaiNongSan).HasColumnName("Ma_Loai_Nong_San");

                entity.Property(e => e.TenLoaiNongSan)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Loai_Nong_San");
            });

            modelBuilder.Entity<Trang_Thai>(entity =>
            {
                entity.HasKey(e => e.Ma_Trang_Thai);

                entity.ToTable("Trang_Thai");

                entity.Property(e => e.Ma_Trang_Thai).HasColumnName("Ma_Trang_Thai");

                entity.Property(e => e.Ten_Trang_Thai)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Trang_Thai");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung);

                entity.ToTable("Nguoi_Dung");

                entity.Property(e => e.MaNguoiDung).HasColumnName("Ma_Nguoi_Dung");

                entity.Property(e => e.Avatar).HasMaxLength(50);

                entity.Property(e => e.DiaChi)
                    .HasMaxLength(50)
                    .HasColumnName("Dia_Chi");

                entity.Property(e => e.MaTaiKhoan).HasColumnName("Ma_Tai_Khoan");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenNguoiDung)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Nguoi_Dung");

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .HasConstraintName("FK_Nguoi_Dung_Tai_Khoan");
               

            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNhanVien);

                entity.ToTable("Nhan_Vien");

                entity.Property(e => e.MaNhanVien).HasColumnName("Ma_Nhan_Vien");

                entity.Property(e => e.Avatar).HasMaxLength(50);

                entity.Property(e => e.DiaChi)
                    .HasMaxLength(50)
                    .HasColumnName("Dia_Chi");

                entity.Property(e => e.MaTaiKhoan).HasColumnName("Ma_Tai_Khoan");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenNhanVien)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Nhan_Vien");

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .HasConstraintName("FK_Nhan_Vien_Tai_Khoan");
            });

            modelBuilder.Entity<NongSan>(entity =>
            {
                entity.HasKey(e => e.MaNongSan);

                entity.ToTable("Nong_San");

                entity.Property(e => e.MaNongSan).HasColumnName("Ma_Nong_San");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.MaDonViTinh).HasColumnName("Ma_Don_Vi_Tinh");

                entity.Property(e => e.MaGianHang).HasColumnName("Ma_Gian_Hang");

                entity.Property(e => e.MaLoaiNongSan).HasColumnName("Ma_Loai_Nong_San");

                entity.Property(e => e.MoTa)
                    .HasMaxLength(50)
                    .HasColumnName("Mo_Ta");

                entity.Property(e => e.SoLuong)
                    .HasMaxLength(50)
                    .HasColumnName("So_Luong");

                entity.Property(e => e.TenNongSan)
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Nong_San");

                entity.Property(e => e.TrongLuong)
                    .HasMaxLength(50)
                    .HasColumnName("Trong_Luong");
                entity.Property(e => e.TrangThai)
                 .HasColumnName("TrangThai");

                entity.HasOne(d => d.MaDonViTinhNavigation)
                    .WithMany(p => p.NongSans)
                    .HasForeignKey(d => d.MaDonViTinh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nong_San_Don_Vi_Tinh");

                entity.HasOne(d => d.MaGianHangNavigation)
                    .WithMany(p => p.NongSans)
                    .HasForeignKey(d => d.MaGianHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nong_San_Gian_Hang");

                entity.HasOne(d => d.MaLoaiNongSanNavigation)
                    .WithMany(p => p.NongSans)
                    .HasForeignKey(d => d.MaLoaiNongSan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nong_San_Loai_Nong_San");
            });

            modelBuilder.Entity<Quyen>(entity =>
            {
                entity.HasKey(e => e.MaQuyen)
                    .HasName("PK_Role");

                entity.ToTable("Quyen");

                entity.Property(e => e.MaQuyen).HasColumnName("Ma_Quyen");

                entity.Property(e => e.TenQuyen)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Quyen");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTaiKhoan);

                entity.ToTable("Tai_Khoan");

                entity.Property(e => e.MaTaiKhoan).HasColumnName("Ma_Tai_Khoan");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MaQuyen).HasColumnName("Ma_Quyen");

                entity.Property(e => e.MatKhau)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mat_Khau");

                entity.Property(e => e.TenDangNhap)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Ten_Dang_Nhap");

                entity.HasOne(d => d.MaQuyenNavigation)
                    .WithMany(p => p.TaiKhoans)
                    .HasForeignKey(d => d.MaQuyen)
                    .HasConstraintName("FK_Tai_Khoan_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
