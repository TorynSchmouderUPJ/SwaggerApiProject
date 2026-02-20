using Newtonsoft.Json;
using PersonApi.Models;

namespace PersonApi.Services {
    public sealed class FileManager {
        // Singleton
        private static readonly Lazy<FileManager> _instance = new Lazy<FileManager>(() => new FileManager());

        public static FileManager Instance => _instance.Value;

        // Internal state
        private readonly string _filePath;
        private readonly object _lock = new object();
        private List<Person> _cache;

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        // Constructor 
        private FileManager() {
            // Store the data file next to the running assembly
            var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
            Directory.CreateDirectory(dataDir);
            _filePath = Path.Combine(dataDir, "persons.json");

            _cache = LoadFromFile();
        }
        // GetAll
        public List<Person> GetAll() {
            lock (_lock) {
                return _cache.Select(p => Clone(p)).ToList();
            }
        }
        
        // GetByID
        public Person? GetById(string id) {
            lock (_lock) {
                var person = _cache.FirstOrDefault(p =>
                    p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                return person is null ? null : Clone(person);
            }
        }

        // Add
        public Person Add(Person person) {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(person.Id))
                {
                    person.Id = Guid.NewGuid().ToString();
                }

                // Prevent dups
                if (_cache.Any(p => p.Id.Equals(person.Id, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException(
                        $"A person with id '{person.Id}' already exists.");
                }

                var entry = Clone(person);
                _cache.Add(entry);
                SaveToFile();
                return Clone(entry);
            }
        }

        // Update
        public Person? Update(string id, Person updated)
        {
            lock (_lock)
            {
                var index = _cache.FindIndex(p =>
                    p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (index == -1) return null;

                _cache[index].Name = updated.Name;
                _cache[index].School = updated.School;
                SaveToFile();
                return Clone(_cache[index]);
            }
        }

        // Delete
        public bool Delete(string id)
        {
            lock (_lock)
            {
                var person = _cache.FirstOrDefault(p =>
                    p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (person is null) return false;

                _cache.Remove(person);
                SaveToFile();
                return true;
            }
        }

        // helpers
        // load
        private List<Person> LoadFromFile() {
            if (!File.Exists(_filePath))
                return new List<Person>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Person>>(json, _jsonSettings)
                   ?? new List<Person>();
        }
        
        // save
        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_cache, _jsonSettings);
            File.WriteAllText(_filePath, json);
        }

        // Clone
        private static Person Clone(Person source) => new Person
        {
            Id = source.Id,
            Name = source.Name,
            School = source.School
        };
    }
}
