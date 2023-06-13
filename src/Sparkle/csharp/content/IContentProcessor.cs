namespace Sparkle.csharp.content; 

public interface IContentProcessor {
    
    public object Load(string path);
    
    public void UnLoad(object content);
}