public class EzFR_Messages
{
    // Titles
    public const string TITLE_00 = "Renaming";
    public const string TITLE_01 = "Attention!";
    public const string TITLE_02 = "Sorting";

    // Messages
    public const string MESSAGE_00 = "Please, wait until the gameObjects are renamed...";
    public const string MESSAGE_01 = "Please, wait until the gameObjects are sorted...";

	// Erros
	public const string ERROR_00 = "There isn't no gameobject selected. Please, select one or more gameobjects and try again.";
	public const string ERROR_01 = "There isn't no file selected. Please, select one or more files and try again.";
	public const string ERROR_02 = "To prevent mistakes it isn't possible to rename files with different extensions. Please, select files with the same extensions and try again.";
	public const string ERROR_03 = "To prevent mistakes it isn't possible to rename with extensions .cs, .js and .shader.";
	public const string ERROR_04 = "To prevent mistakes it isn't possible to sort gameObjects that has different parents.";
	public const string ERROR_05 = "To prevent mistakes it isn't possible to sort the children of multiple gameObjects.";
    public const string ERROR_06 = "Internal Error. GameObject and Object can't be both null.";

	// Warnings
	public const string WARNING_00 = "One or more gameobjects has different parents and this may cause a problem on your Hierarchy. Do want to continue?";
	public const string WARNING_01 = "It isn't possible to has files with the same name inside Project Folder. Make Sequential has been activated and initial number is 0.";

    // Buttons
    public const string BUTTON_00 = "Close";
    public const string BUTTON_01 = "Cancel";
    public const string BUTTON_02 = "Continue";
}
