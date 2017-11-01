/* JamieDB: Testprogramm zur Vorarbeit im Projekt "Jamie". Die zweite Version ist eine
 * WPF-Anwendung mit rudimentärem Userinterface. Die Architektur folgt dem Model-View-ViewModel Prinzip.
 * 
 * Author: Klaus Christochowitz  01-2017
 * 
 * Version 0.01 - 2017-01-17: Erste WPF-Testversion in der MVVM-Architektur: 
 *                            - auf geht's
 *                            
 * Version 0.02 - 2017-01-19: WPF Tests
 * 
 * Version 0.03 - 2017-01-22: Schreibenden Zugriff auf die Datenban implementiert
 *                            - Bindings Etabliert
 *                            - Command: SaveRecipe in MaintenanceRecipes hinzu
 *                            - Unit in View für Rezeptzutat hinzugefügt
 *                            - Command "Rezept zufügen" hinzu
 *                            
 * Version 0.04 - 2017-01-29: Command Klasse für das ViewModel implementiert: JamieDBViewModelCommand
 *                            - SaveCommand umgestellt 
 *                              von  JamieDB.ViewModel.Command.SaveCommand 
 *                              nach JamieDB.ViewModel.JamieDBViewModelCommand;
 *                            - NewRecipeCommand umgestellt 
 *                            
 * Version 0.05 - 2017-01-29: Alte Command Klassen beseitigt und durch JamieDBViewModelCommand ersetzt.
 *                            - NewRecipeIngredientCommand umgestellt
 *                            - Implementierung Ingredient - Reiter begonnen
 *                            
 * Version 0.06 - 2017-02-06: Properties im ViewModel: Listen umgestellt von IEnumerable auf ObservableCollection
 *                            - Implementierung Ingredient.
 *                            
 * Version 0.07 - 2017-02-13: - NewIngredient Command hinzu
 *                            - NewUnit Command hinzu
 *                            - Eventhandler für RecipeIngredients hinzu
 *                            
 * Version 0.08 - 2017-02-15: - DeleteRecipe / DeleteRecipeIngredient Command hinzu
 * 
 * Version 0.09 - 2017-02-16: - DeleteIngredient / DeleteUnit Command hinzu
 * 
 * Version 0.10 - 2017-02-17: Start FoodPlan Implementation:
 *                            - XML: DatePicker - Detail Datagrid added (FoodPlanItems)
 *                            - ViewModel: SelectedFoodPlanDate added
 *
 * Version 0.11 - 2017-02-19: FoodPlan Implementation
 *                            - SelectedFoodPlanItem added (toBeDeleted)
 *                            - Commands added: NewFoodPlanItem, DeleteFoodPlanItems
 *                            Shopping List
 *                            - ViewModel Property "ShoppingListItems" added
 *                            - WPF XML: Tab "Shopping List" added
 *                            
 * Version 0.12 - 2017-02-27: FoodPlan Template Implementation
 *                            - SelectedFoodPlanTemplateEndDate added
 *                            - NewFoodPlanTemplateCommand implemented
 *                            - DeleteFoodPlanTemplateCommand implemented
 *                            - LoadFoodPlanTemplateCommand implemented
 * Version 0.13 - 2017-03-02: Unit Translations
 *                            - Command NewUnitTranslation added
 *                            - UnitTranslation bound to XML
 *                            
 *                            Bugfixes
 *                            - XML-TabItem Units: UnitType Combobox
 *                            - XML-TabItem Ingredients: TargetUnit & Type Combobox
 */

/* Version 0.14 - 2017-03-xx: 
 *                            - 
 *                            
 *                            Bugfixes
 *                            - xxx=Getxxx --> Refreshxxx()
 *
 *                      open: - Add Inverse UnitTranslations to Table (l-->ml) => (ml -->l)
 *                            - Edit Inverse UnitTranslations
 *                            - Enter Test Data to Unit Translations
 *                            - Implement Unit Translation Calculation
 *                            - 
 *                            
 *            open Questions: - Right Orientation DataGridComboBoxColumn
 *                            - Umstellung auf SQL Authentication
 *                            - 
 *
 * 
 *               Checklists : 1. <Name Programmiercheckliste>
 *                            -
 */

/*[Flags] public enum TranslationType
{ IsTypeChange = 0x1, IsIngredientDependent = 0x2 }
*/

/* UnitTranslation - Grundsätze
 * 
 * 1. Es sollte alle Umrechnungen geben vom Typ "Immer gültig"
 * 2. Je IngredientType (Flüssig, Fest, Pulver, Kräuter, ....)
 *    gibt es genau eine Default-Umrechnung für jede Kombination an UnitTypes
 * 3. Wird eine Defaultumrechnung verwendet, dann wird diese für die Zutat protokolliert, 
 *    damit die fehlende Zutaten spezifische Umrechnung nachgetragen und verifiziert werden kann
 * 4. Für jede Zutat und Typenwechsel darf es nur einen "Von Zutat abhängiger UnitTypenWechsel" geben
 * 5. Für jede Zutat und Typenwechsel können unter Berücksichtigung der Einträge "Immer gültig"
 *    alle weiteren notwendigen Umrechnungsfaktoren berechnet werden. 
 * 6. Synonyme bei Units können über UnitTranslations mit dem TranslationFactor 1,0 eingegeben werden.
 *    
 * Beschreibung der Umrechnung für eine Zutat:
 * 1. Ermittle StartUnit aus IngredientItem.Unit
 * 2. Ermittle ZielUnit aus Ingredient.TargetUnit
 * 3. Vergleiche UnitType
 *    3a  UnitType gleich
 *    3a1 Ermittle UnitTranslation Fall 0 mit StartUnit und ZielUnit
 *    3a2 Falls vorhanden --> Rechne um
 *    3a3 Falls nicht vorhanden --> Meldung "Umrechnung fehlt."
 *    
 * 
 * 
 * TranslationIndependenceType Flags - Anwendungsfälle
 * 
 * Fall 0: - Immer gültig 
 *         - Wert = 0,  <kein Flag gesetzt>
 *         - Unabhängig von der Zutat, UnitTypen sind gleich
 *         - Ingredient muss gleich >null< sein 
 *         - IngredientType muss gleich >null< sein 
 *         - Umrechnung gilt immer: Beispiel 1kg --> 1000g
 *           
 * Fall 1: - Defaultumrechnung, wenn noch kein spez. Eintrag für die Zutat besteht
 *         - Wert = 1, IsTypeChange
 *         - UnitTypen sind verschieden        
 *         - Ingredient muss gleich >null< sein 
 *         - IngredientType muss ungleich >null< sein 
 *         - z.B. 1l --> 1kg
 * 
 * Fall 2: - Wert = 2: - wird nicht verwendet
 * 
 * Fall 3: - "Von Zutat abhängiger UnitTypenWechsel", wenn noch kein spez. Eintrag für die Zutat besteht
 *         - Wert = 3, IsTypeChange, IsIngredientDependent
 *         - UnitTypen sind verschieden
 *         - Abhängig von der Zutat
 *         - Zutat muss ungleich >null< sein 
 *         - IngredientType spielt keine Rolle, sollte aber mit dem IngredientType der Zutat übereinstimmen
 *         - z.B. 1TL Salz --> 9g
 */

/* GetTranslation - Versions
* -------------------------
* 
* GetTranslation (BaseUnit, TargetUnit, Ingredient)
* - Calculates Translation Base->Target for Ingredient 
* - if possible - otherwise Create Suggestion and return null
* 
* GetTranslation (BaseUnit, TargetUnit, Ingriedient.Type)
* - Calculates Translation Base->Target for Ingredient.Type (if possible - otherwise null)
* - if possible - otherwise Create Suggestion and return null
* 
* GetTranslation (BaseUnit, TargetUnit)
* - Calculates Translation Base->Target, if Base and Target have same Unit.Type 
* - if possible - otherwise Calculate missing entry and return null
* 
* AddInactiveTranslation (BaseUnit, TargetUnit, Ingredient)
* - Creates UnitTranslation Base->Target for Ingredient to be verified.
* 
* AddInactiveTranslation (BaseUnit, TargetUnit, Ingredient.Type)
* - Creates UnitTranslation Base->Target for Ingredient.Type to be verified.
* 
*/

/*    [Flags] public enum IngredientFlags : int
    { IsVegetarian = 1, IsVegan = 2, IsLowCarb = 4, IsLowFat = 8 } 
    */

/*    public enum IngredientType : int { IsFluid, IsSolid, IsCrystal, IsPowder, IsHerb, IsGranular, NotInitialized = 999 }
*/