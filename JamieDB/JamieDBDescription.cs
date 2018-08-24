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
 * Version 0.14 - 2018-05-02: Unit Translations
 *                            - Reorganization: Unit Translation Factor splitted in Table UnitTranslation (different type)
 *                              and Unit (same type)
 *                            JamieDBViewModelBase 
 *                            - Base class for all ViewModel classes created
 *                            - JamieDBEieModelBase applied to new VM classes UnitVMClass, IngredientVMClass
 *                            SQL Server
 *                            - Connection changed to IP
 *                            - Login Changed to JamieDBUser
 * Version 0.15 - 2018-05-06: 
 *                            - Einheitenumrechnung Implementiert (noch nicht getestet) 
 *                            - Überarbeitung Eingabe UnitTranslations begonnen
 *                            
 * Version 0.16 - 2018-08-24: 
 *                            - View: Tab FoodPlan added
 *                            - ViewModel: FoodPlansVM added
 *                            - ViewModel: FoodPlansVM Command - "NewFoodPlan" added
 *                            - Model: FoodPlan Class - GetNextFoodPlanItemDate
 */

/* Version 0.17 - 2018-08-xx: 
 *                            - 
 *                            
 *                            Bugfixes
 *                            - 
 *
 *                      open: - Units: change Standard Unit
 *                            - UnitTranslations: CreateMissingBasicTranslations
 *                            - UnitTranslation.AddBasicUnitTranslations(); läuft nicht richtig
 *                            - 
 *                            - 
 *                            
 *            open Questions: - 
 *                            - 
 *                            - 
 *
 * 
 *               Checklists : 1. <Name Programmiercheckliste>
 *                            -
 *                            
 *                    To DOs: - Refresh Methoden umbenennen nach GetXXFromDatabase
 *                            -      
 *                            
 *                   Planung: - Einheiten - Umrechnung finalisieren
 *                            - Generieren von Einkaufslisten
 *                            - User-Verwaltung: Rumpf erzeugen...
 *                            - Produkteverwaltung
 *                            - Bezugsquellen
 */

/*[Flags] public enum TranslationType
{ IsTypeChange = 0x1, IsIngredientDependent = 0x2 }
*/

