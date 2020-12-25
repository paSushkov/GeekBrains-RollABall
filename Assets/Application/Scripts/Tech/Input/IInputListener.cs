namespace LabyrinthGame.Tech.Input
{
    public interface IInputListener
    {
        float Horizontal { get;}
        float Vertical { get;}
        float Cancel { get;}
        float Fire1 { get;}
        float Jump { get;}
        float Save { get;}
        float Load { get;}
        

        void Initialize(IInputTranslator translator);
        void Shutdown(IInputTranslator translator);
    }
}