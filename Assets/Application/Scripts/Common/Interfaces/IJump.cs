namespace LabyrinthGame.Common.Interfaces
{
    public interface IJump
    {
        float JumpPower { get; set; }
        void Jump();
    }
}