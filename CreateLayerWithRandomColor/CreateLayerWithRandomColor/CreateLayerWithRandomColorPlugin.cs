using Rhino;
using System;
using System.Drawing;
using System.Reflection;
using System.Linq;
using Rhino.DocObjects.Tables;

namespace CreateLayerWithRandomColor
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class CreateLayerWithRandomColorPlugin : Rhino.PlugIns.PlugIn
    {
        public CreateLayerWithRandomColorPlugin()
        {
            Instance = this;
            RhinoDoc.LayerTableEvent += RandomizeLayerColor;
        }

        ///<summary>Gets the only instance of the CreateLayerWithRandomColorPlugin plug-in.</summary>
        public static CreateLayerWithRandomColorPlugin Instance { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.

        private static void RandomizeLayerColor(object sender, LayerTableEventArgs e)
        {
            if (e.EventType == LayerTableEventType.Added)
            {
                PropertyInfo[] colors = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);
                Color color = (Color)colors.OrderBy(c => Guid.NewGuid()).FirstOrDefault().GetValue(null);
                e.NewState.Color = color;
            }
        }
    }
}