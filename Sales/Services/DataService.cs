namespace Sales.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            _ = this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasepath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasepath);
            await connection.CreateTableAsync<Product>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }
        public async Task Insert<T>(List<T> model)
        {
            await this.connection.InsertAllAsync(model);
        }
        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }
        public async Task Update<T>(List<T> model)
        {
            await this.connection.UpdateAllAsync(model);
        }
        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var query = await this.connection.QueryAsync<Product>("SELECT * FROM [Product]");
            var array = query.ToArray();
            var list = array.Select(p => new Product
            {
                Description = p.Description,
                ImagePath = p.ImagePath,
                IsAvailable = p.IsAvailable,
                Price = p.Price,
                ProductId = p.ProductId,
                PublishOn = p.PublishOn,
                Remarks = p.Remarks,
            }).ToList();
            return list;
        }
        public async Task DeleteAllProduct()
        {
            var query = await this.connection.QueryAsync<Product>("DELETE FROM [Product]");
        }
    }
}
