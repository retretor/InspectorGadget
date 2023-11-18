// namespace InspectorGadget.Db;
//
// public class DbRepository
// {
//     private readonly RentDbContext _context;
//
//     public DbRepository()
//     {
//         _context = new RentDbContext(new DbContextOptions<RentDbContext>());
//         _context.Database.EnsureCreated();
//     }
//     
//     public async Task<IEnumerable<T>?> GetAllAsync<T>() where T : class
//     {
//         var entities = await _context.Set<T>().ToListAsync();
//         if (entities.Count == 0)
//         {
//             Console.WriteLine($"No {typeof(T).Name} found");
//             return null;
//         }
//
//         Console.WriteLine($"{typeof(T).Name} count: {entities.Count}");
//         return entities;
//     }
//     
//     public async Task<T?> GetAsync<T>(int id) where T : class
//     {
//         var entities = await _context.Set<T>().ToListAsync();
//         var entity = entities.FirstOrDefault(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
//         if (entity == null)
//         {
//             Console.WriteLine($"{typeof(T).Name} not found: {id}");
//             return null;
//         }
//         
//         return entity;
//     }
//     
//     public async Task<T?> UpdateAsync<T>(int id, object dto) where T : class
//     {
//         var entities = await _context.Set<T>().ToListAsync();
//         var entityToUpdate = entities.FirstOrDefault(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
//         if (entityToUpdate == null)
//         {
//             Console.WriteLine($"{typeof(T).Name} not found: {id}");
//             return null;
//         }
//
//         var dtoProperties = dto.GetType().GetProperties();
//         foreach (var property in dtoProperties)
//         {
//             var entityProperty = entityToUpdate.GetType().GetProperty(property.Name);
//             if (entityProperty != null && entityProperty.CanWrite)
//             {
//                 entityProperty.SetValue(entityToUpdate, property.GetValue(dto));
//             }
//         }
//
//         await _context.SaveChangesAsync();
//         return entityToUpdate;
//     }
//     
//     public async Task<T?> CreateAsync<T>(object dto) where T : class
//     {
//         var entities = await _context.Set<T>().ToListAsync();
//         var id = (int)(dto.GetType().GetProperty("Id")?.GetValue(dto) ?? 0);
//         if (entities.Any(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true))
//         {
//             Console.WriteLine($"{typeof(T).Name} already exists: {id}");
//             return null;
//         }
//
//         var entityType = typeof(T);
//         var constructor = entityType.GetConstructor(dto.GetType().GetProperties().Select(p => p.PropertyType).ToArray());
//         if (constructor != null)
//         {
//             var constructorParams = dto.GetType().GetProperties().Select(p => p.GetValue(dto)).ToArray();
//             var newEntity = constructor.Invoke(constructorParams);
//             _context.Set<T>().Add((T)newEntity);
//             await _context.SaveChangesAsync();
//             return (T)newEntity;
//         }
//
//         return null;
//     }
//
//     public async Task DeleteAsync<T>(int id) where T : class
//     {
//         var entities = await _context.Set<T>().ToListAsync();
//         var entityToDelete = entities.FirstOrDefault(e => e.GetType().GetProperty("Id")?.GetValue(e)?.Equals(id) == true);
//         if (entityToDelete == null)
//         {
//             Console.WriteLine($"{typeof(T).Name} not found: {id}");
//             return;
//         }
//
//         _context.Set<T>().Remove(entityToDelete);
//         await _context.SaveChangesAsync();
//     }
// }