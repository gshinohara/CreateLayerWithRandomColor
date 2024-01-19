using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace CreateLayerWithRandomColor
{
    public class NewLayerCommand : Command
    {
        public NewLayerCommand()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static NewLayerCommand Instance { get; private set; }

        public override string EnglishName => "NewLayer";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            GetInteger gi = new GetInteger();
            gi.AcceptNothing(true);
            gi.SetCommandPrompt("Input how many layers you want.");
            ParentLayer current = ParentLayer.None;
            gi.AddOptionEnumList("Parent", current);

            while (true)
            {
                switch (gi.Get())
                {
                    case GetResult.Cancel:
                        return Result.Cancel;
                    case GetResult.Option:
                        current = (ParentLayer)gi.Option().CurrentListOptionIndex;
                        break;
                    case GetResult.Number:
                        int num = gi.Number();
                        var layers = RhinoDoc.ActiveDoc.Layers;
                        switch (current)
                        {
                            case ParentLayer.None:
                                for (int i = 0; i < num; i++)
                                    layers.Add();
                                break;
                            case ParentLayer.ActiveLayer:
                                for (int i = 0; i < num; i++)
                                {
                                    int li = layers.Add();
                                    layers.FindIndex(li).ParentLayerId = layers.CurrentLayer.Id;
                                }
                                break;
                            default:
                                return Result.Failure;
                        }
                        return Result.Success;
                }
            }
        }
    }
    internal enum ParentLayer
    {
        None,
        ActiveLayer
    }
}