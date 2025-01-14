namespace Sparkle.csharp.content.type; 

public class ModelContent : IContentType {
    
    public string Path { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the ModelContent class with the specified path.
    /// </summary>
    /// <param name="path">The path to the model content.</param>
    public ModelContent(string path) {
        this.Path = path;
    }
}