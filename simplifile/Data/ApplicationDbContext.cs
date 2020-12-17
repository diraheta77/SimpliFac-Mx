using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using simplifile.Models;


namespace simplifile.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<UnidadNegocio> UnidadadesNegocios { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<RequisicionSat> Requisiciones { get; set; }
        public DbSet<Parm> Parms { get; set; }
        public DbSet<MasterParm> MasterParms { get; set; }

        public DbSet<CFDIDatos> CFDIs { get; set; }

        public DbSet<Nomina> Nominas { get; set; }
        public DbSet<NominaDetalle> NominaDetalles { get; set; }

        public DbSet<ConNomina> ConNominas { get; set; }

        public DbSet<ConfigVistaUsuario> ConfigVistas { get; set; }

        public DbSet<CFDIPagados> CFDIPagados { get; set; }

        public DbSet<CFDIDetalle> CFDIDetalles { get; set; }

        public DbSet<CRPDatos> CRPDatos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CFDIPagados>(t =>
            {
                t.ToTable("CFDIPagados");
                t.HasKey(p => new { p.CfdUUID, p.CfdReferenciaPago });
            });

            builder.Entity<Usuario>( t=>
            {
                t.ToTable("Usuarios");
                t.HasKey(p => new { p.UsuId});
                t.HasOne(p => p.Empresa).WithMany(b => b.Usuarios).HasForeignKey(p => p.UsuEmpId);
            });

            builder.Entity<Empresa>(t =>
            {
                t.ToTable("Empresas");
                t.HasMany(p => p.UnidadadesNegocios).WithOne(b => b.Empresa).HasForeignKey(p => p.EmpId);
                t.HasMany(p => p.Requisiciones).WithOne(b => b.Empresa).HasForeignKey(p => p.EmpId);
                t.HasMany(p => p.Parms).WithOne(b => b.Empresa).HasForeignKey(p => p.EmpId);
            });

            builder.Entity<UnidadNegocio>(t =>
            {
                t.ToTable("UnidadNegocio");
                t.HasKey(p => new { p.UnId,p.EmpId });
                t.HasMany(p => p.Requisiciones).WithOne(b => b.UnidadNegocio).HasForeignKey(p => new { p.UnId ,p.EmpId });
                t.HasMany(p => p.Certificados).WithOne(b => b.UnidadNegocio).HasForeignKey(p => new { p.UnId, p.EmpId } );
            });

            builder.Entity<Certificado>(t =>
            {
                t.ToTable("Certificados");
                t.HasKey(p => new { p.CerId });
            });

            builder.Entity<Parm>(t =>
            {
                t.ToTable("Parms");
                t.HasKey(p => new { p.ParmId, p.EmpId, p.UnId });
            });

            builder.Entity<MasterParm>(t =>
            {
                t.ToTable("MasterParms");
                t.HasKey(p => new { p.MParmId });
            });

            builder.Entity<RequisicionSat>(t =>
            {
                t.ToTable("RequisicionesSAT");
                t.HasKey(p => new { p.RsId });
            });

            builder.Entity<CFDIDatos>(t =>
            {
                t.ToTable("CFDIDatos");
                t.HasKey(p => new { p.CfdUUID });
            });

            builder.Entity<Nomina>(t =>
            {
                t.ToTable("CRNNomina");
                t.HasKey(p => new { p.CrnUUID });
                //t.Property(d=>d.CrnUUID)
            });

            builder.Entity<NominaDetalle>(t =>
            {
                t.ToTable("CRNNominaDetalle");
                t.HasKey(d => new { d.CrnUUID, d.CrnNaturaleza, d.CrnTipo, d.CrnClave });
                t.HasOne(d => d.nomina).WithMany(k => k.nominaDetalles).HasForeignKey(d => d.CrnUUID);
            });

            builder.Entity<ConNomina>(t =>
            {
                t.ToTable("ConNomina");
                t.HasKey(d => new { d.CnnUUID });
            });

            builder.Entity<ConfigVistaUsuario>(t =>
            {
                t.ToTable("ConfigVistaUsuario");
                t.HasKey(d => new { d.Usuario });
            });

            builder.Entity<CFDIDetalle>(t =>
            {
                t.ToTable("CFDIDetalle");
//                t.HasKey(p => new { p.CfdUUID, p.CfdReferenciaPago });
            });

            builder.Entity<CRPDatos>(t =>
            {
                t.ToTable("CRPDatos");
                t.HasKey(p => new { p.CrpUUID, p.CrpIdDocumento });
            });

        }
    }
}
