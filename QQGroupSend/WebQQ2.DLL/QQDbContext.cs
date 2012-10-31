using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Format.WebQQ.Model.Messages;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data;

namespace Format.WebQQ.WebQQ2.DLL
{
    public class QQDbContext : DbContext, IQQDbContext
    {

        #region TPC
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //base.OnModelCreating(modelBuilder);
        //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<QQDbContext>());

        //    modelBuilder.Entity<GroupMessage>()
        //        .Property(p => p.ID)
        //        .IsRequired()
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        //    modelBuilder.Entity<GroupMessage>()
        //        .HasKey(p => p.ID)
        //        .Map(m =>
        //        {
        //            m.MapInheritedProperties();
        //            m.ToTable("GroupMessage");
        //        });

        //    modelBuilder.Entity<JoinGroupRequestMessage>()
        //        .Map(m =>
        //        {
        //            m.MapInheritedProperties();
        //            m.ToTable("JoinGroupRequestMessage");
        //        })
        //        .HasKey(p => p.ID)
        //        ;

        //    //modelBuilder.Entity<GroupSignMessage>()
        //    //    .HasKey(p => p.ID)
        //    //    .Map(m =>
        //    //    {
        //    //        m.MapInheritedProperties();
        //    //        m.ToTable("GroupSignMessage");
        //    //    })
        //    //    .HasKey(p => p.ID);

        //}
        #endregion

        #region TPT
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<QQDbContext>());
            modelBuilder.Entity<GroupMessage>().Property(m => m.Message).HasMaxLength(2000);
            modelBuilder.Entity<GroupMessage>().ToTable("GroupMessage");
            modelBuilder.Entity<JoinGroupRequestMessage>().ToTable("JoinGroupRequestMessage");
        }
        #endregion


        #region IQQDbContext 成员
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<JoinGroupRequestMessage> JoinGroupRequestMessages { get; set; }

        public void Submit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine("\t\tProperty: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (DataException dEx)
            {
                Console.WriteLine(dEx);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion
    }

    public interface IQQDbContext
    {
        DbSet<GroupMessage> GroupMessages { set; get; }
        DbSet<JoinGroupRequestMessage> JoinGroupRequestMessages { set; get; }
        void Submit();
    }

}
