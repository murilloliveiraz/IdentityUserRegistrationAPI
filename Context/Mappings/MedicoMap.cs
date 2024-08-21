using APIThatSendAnEmailToUpdatePassword.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace APIThatSendAnEmailToUpdatePassword.Context.Mappings
{
    public class MedicoMap : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.ToTable("medicos")
            .HasKey(m => m.Id);

            builder.Property(m => m.Especialidade)
            .HasColumnType("VARCHAR");

            builder.Property(m => m.CRM)
            .HasColumnType("VARCHAR")
            .IsRequired();

            builder.Property(m => m.DataCriacao)
            .HasColumnType("timestamp")
            .IsRequired();

            builder.Property(m => m.DataInativacao)
            .HasColumnType("timestamp");

            builder.Property(m => m.UserId)
                .HasColumnType("VARCHAR")
                .IsRequired();

            builder.HasOne(m => m.Usuario)
               .WithOne()
               .HasForeignKey<Medico>(m => m.UserId) // Define o campo UserId como chave estrangeira
               .OnDelete(DeleteBehavior.Cascade); // Define o comportamento de exclusão em cascata
        }
    }
}
