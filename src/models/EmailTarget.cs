namespace VontobelTest.src.models
{
   public record EmailTarget(string Target, string TargetType, string TargetFormat): ITarget;

}