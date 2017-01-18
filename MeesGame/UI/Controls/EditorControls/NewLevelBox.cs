using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class NewLevelBox : Texture
    {
        public event OnClickEventHandler Succes;

        private const int edgeThickness = 10;
        private const int padding = 20;

        private NumberInputBox columnsEdit;
        private NumberInputBox rowsEdit;

        private UIComponent innerComponentsContainer;

        public NewLevelBox(Location location, int startNumRows, int startNumColumns) : base(location, null, null, null)
        {
            Dimensions = new WrapperDimensions(edgeThickness * 2 + padding, edgeThickness * 2 + padding, true, true);

            innerComponentsContainer = new UIComponent(CenteredLocation.All, WrapperDimensions.All);

            columnsEdit = new NumberInputBox(SimpleLocation.Zero, new CombinationDimensions(InheritDimensions.All, new MeasuredDimensions()), Strings.columns_edit_text, startNumColumns, 1, spritefont: DefaultUIValues.Default.DefaultEditorControlSpriteFont);
            rowsEdit = new NumberInputBox(new RelativeToLocation(columnsEdit, 0, 10, true, false), new CombinationDimensions(InheritDimensions.All, new MeasuredDimensions(rowsEdit)), Strings.rows_edit_text, startNumRows, 1, spritefont: DefaultUIValues.Default.DefaultEditorControlSpriteFont);

            UIComponent centerSpanner = new UIComponent(new CombinationLocation(new CenteredLocation(horizontalCenter: true), new RelativeToLocation(rowsEdit, 0, 50, true, false)), WrapperDimensions.All);

            Button okButton = new SpriteSheetButton(SimpleLocation.Zero, null, Strings.ok, onClick: (UIComponent component) =>
            {
                Succes?.Invoke(this);
            });
            Button cancelButton = new SpriteSheetButton(new RelativeToLocation(okButton, 40, 0 , false), null, Strings.cancel, onClick: (UIComponent component) =>
            {
                Visible = false;
            });

            centerSpanner.AddConstantComponent(okButton);
            centerSpanner.AddConstantComponent(cancelButton);

            innerComponentsContainer.AddConstantComponent(columnsEdit);
            innerComponentsContainer.AddConstantComponent(rowsEdit);
            innerComponentsContainer.AddConstantComponent(centerSpanner);

            AddConstantComponent(innerComponentsContainer);

            CurrentTexture = Utility.EdgeTexture(CurrentDimensions.X, CurrentDimensions.Y, edgeThickness, Utility.DrawingColorToXNAColor(DefaultUIValues.Default.BoxEdgeColor), Utility.DrawingColorToXNAColor(DefaultUIValues.Default.BoxInnerColor));
        }

        public int Columns
        {
            get { return columnsEdit.Number; }
        }

        public int Rows
        {
            get { return rowsEdit.Number; }
        }
    }
}
