using API.Contracts;
using API.Data;
using API.Utilities.Handlers;

namespace API.Repositories;
/*
 * Definisi Generic Class GeneralRepository yang mengimplementasikan interface IGeneralRepository
 * Kelas ini untuk melakukan operasi CRUD ke dalam database
 * `TEntity` dibatasi hanya berupa kelas 
 */
public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
{
    protected readonly BookingManagementDbContext _context;

    public GeneralRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    /*
     * <summary> Digunakan untuk mengambil semua data entitas dari database</summary>
     * <returns> Data akan dikembalikan sebagai IEnumerable </returns>
     */
    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList(); //mengambil semua data sebuah entitas dari database
    }

    /*
     * <summary>Digunakan untuk mengambil data entitas dengan Guid tertentu</summary>
     * <params name="guid">Guid dari entitas yang akan dikembalikan</params>
     * <returns>Mengembalikan sebuah entitas</returns>
     */
    public TEntity GetByGuid(Guid guid)
    {
        var entity = _context.Set<TEntity>().Find(guid); //mengambil data entitas dengan Guid tertentu dari database
        _context.ChangeTracker.Clear(); //Membersihkan ChangeTracker untuk menhindari error saat key value(guid) sudah di-track, contohnya saat update
        return entity; 
    }

    /*
     * <summar> Digunakan untuk memasukkan data entitas ke dalam database</summary>
     * <params name="entity">Data entitas yang akan dimasukkan ke database</params>
     * 
     */
    public TEntity Create(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity); //menambahkan data ke dalam database
            _context.SaveChanges(); //menyimpan perubahan, seperti commit
            return entity;
        }
        catch (Exception ex)
        {
            /*
             * Melemparkan exception berupa objek ExceptionHandler
             * Jika InnerException tidak bernilai null maka parameternya adalah Message dari InnerException
             * Jika InnerException bernilai null maka akan parameternya adalah Message dari Exception
             */
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }

    /*
     * <summary>Digunakan untuk update data dari entitas pada database</summary>
     * <params name="entity">perubahan data</params>
     * <returns>
     * Jika perubahan data berhasil akan mengembalikan nilai true
     * Jika terjadi error atau data gagal diupdate maka akan melempar objek ExceptionHandler
     * <return>
     */
    public bool Update(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity); //Melakukan perubahan data ke database
            _context.SaveChanges(); //Menyimpan perubahan, seperti commit
            return true; 
        }
        catch (Exception ex)
        {
            /*
             * Melemparkan exception berupa objek ExceptionHandler
             * Jika InnerException tidak null maka parameternya adalah Message dari InnerException
             * Jika InnerException bernilai null maka akan parameternya adalah Message dari Exception
             */
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }

    /*
     * <summary>Digunakan untuk menghapus data dari sebuah entitas pada database</summary>
     * <params name="entity">Data entitas yang akan dihapus</params>
     * <returns>
     * Jika data berhasil dihapus akan mengembalikan nilai true
     * JIka terjadi error atau gagal dihapus akan melemparkan object ExceptionHandler
     */
    public bool Delete(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Remove(entity); //Menghapus data tertentu dari database
            _context.SaveChanges(); //menyimpan perubahan, seperti commit
            return true;
        }
        catch (Exception ex)
        {
            /*
             * Melemparkan exception berupa objek ExceptionHandler
             * Jika InnerException tidak null maka parameternya adalah Message dari InnerException
             * Jika InnerException bernilai null maka akan parameternya adalah Message dari Exception
             */
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }
}

