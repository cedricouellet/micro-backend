namespace ExampleConsumer.Models
{
    /// <summary>
    /// Serves as an example model 
    /// </summary>
    public record ExampleItem
    {
        /// <summary>
        /// Gets the ID of the model
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Gets or sets the name of the model
        /// </summary>
        public string Name { get; set; } = default!;
        
        /// <summary>
        /// Gets the UTC date of the model's creation
        /// </summary>
        public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
        
        /// <summary>
        /// Gets a random number attributed to the model (misc)
        /// </summary>
        public int RandomNumber { get; } = new Random().Next(0, int.MaxValue);
    }
}
