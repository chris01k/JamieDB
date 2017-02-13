﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JamieDB.Model
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="JamieDB")]
	public partial class JamieDBLinqDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertIngredient(Ingredient instance);
    partial void UpdateIngredient(Ingredient instance);
    partial void DeleteIngredient(Ingredient instance);
    partial void InsertUnit(Unit instance);
    partial void UpdateUnit(Unit instance);
    partial void DeleteUnit(Unit instance);
    partial void InsertIngredientType(IngredientType instance);
    partial void UpdateIngredientType(IngredientType instance);
    partial void DeleteIngredientType(IngredientType instance);
    partial void InsertRecipeIngredient(RecipeIngredient instance);
    partial void UpdateRecipeIngredient(RecipeIngredient instance);
    partial void DeleteRecipeIngredient(RecipeIngredient instance);
    partial void InsertRecipe(Recipe instance);
    partial void UpdateRecipe(Recipe instance);
    partial void DeleteRecipe(Recipe instance);
    partial void InsertUnitType(UnitType instance);
    partial void UpdateUnitType(UnitType instance);
    partial void DeleteUnitType(UnitType instance);
    #endregion
		
		public JamieDBLinqDataContext() : 
				base(global::JamieDB.Properties.Settings.Default.JamieDBConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public JamieDBLinqDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public JamieDBLinqDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public JamieDBLinqDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public JamieDBLinqDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Ingredient> Ingredients
		{
			get
			{
				return this.GetTable<Ingredient>();
			}
		}
		
		public System.Data.Linq.Table<Unit> Units
		{
			get
			{
				return this.GetTable<Unit>();
			}
		}
		
		public System.Data.Linq.Table<IngredientType> IngredientTypes
		{
			get
			{
				return this.GetTable<IngredientType>();
			}
		}
		
		public System.Data.Linq.Table<RecipeIngredient> RecipeIngredients
		{
			get
			{
				return this.GetTable<RecipeIngredient>();
			}
		}
		
		public System.Data.Linq.Table<Recipe> Recipes
		{
			get
			{
				return this.GetTable<Recipe>();
			}
		}
		
		public System.Data.Linq.Table<UnitType> UnitTypes
		{
			get
			{
				return this.GetTable<UnitType>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Ingredients")]
	public partial class Ingredient : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private System.Nullable<long> _TypeID;
		
		private System.Nullable<long> _TargetUnitID;
		
		private System.Nullable<bool> _IsVegan;
		
		private System.Nullable<bool> _IsVegetarian;
		
		private System.Nullable<bool> _IsLowCarb;
		
		private System.Nullable<bool> _IsLowFat;
		
		private EntitySet<RecipeIngredient> _RecipeIngredients;
		
		private EntityRef<IngredientType> _IngredientType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnTypeIDChanging(System.Nullable<long> value);
    partial void OnTypeIDChanged();
    partial void OnTargetUnitIDChanging(System.Nullable<long> value);
    partial void OnTargetUnitIDChanged();
    partial void OnIsVeganChanging(System.Nullable<bool> value);
    partial void OnIsVeganChanged();
    partial void OnIsVegetarianChanging(System.Nullable<bool> value);
    partial void OnIsVegetarianChanged();
    partial void OnIsLowCarbChanging(System.Nullable<bool> value);
    partial void OnIsLowCarbChanged();
    partial void OnIsLowFatChanging(System.Nullable<bool> value);
    partial void OnIsLowFatChanged();
    #endregion
		
		public Ingredient()
		{
			this._RecipeIngredients = new EntitySet<RecipeIngredient>(new Action<RecipeIngredient>(this.attach_RecipeIngredients), new Action<RecipeIngredient>(this.detach_RecipeIngredients));
			this._IngredientType = default(EntityRef<IngredientType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeID", DbType="BigInt")]
		public System.Nullable<long> TypeID
		{
			get
			{
				return this._TypeID;
			}
			set
			{
				if ((this._TypeID != value))
				{
					if (this._IngredientType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnTypeIDChanging(value);
					this.SendPropertyChanging();
					this._TypeID = value;
					this.SendPropertyChanged("TypeID");
					this.OnTypeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TargetUnitID", DbType="BigInt")]
		public System.Nullable<long> TargetUnitID
		{
			get
			{
				return this._TargetUnitID;
			}
			set
			{
				if ((this._TargetUnitID != value))
				{
					this.OnTargetUnitIDChanging(value);
					this.SendPropertyChanging();
					this._TargetUnitID = value;
					this.SendPropertyChanged("TargetUnitID");
					this.OnTargetUnitIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsVegan", DbType="Bit")]
		public System.Nullable<bool> IsVegan
		{
			get
			{
				return this._IsVegan;
			}
			set
			{
				if ((this._IsVegan != value))
				{
					this.OnIsVeganChanging(value);
					this.SendPropertyChanging();
					this._IsVegan = value;
					this.SendPropertyChanged("IsVegan");
					this.OnIsVeganChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsVegetarian", DbType="Bit")]
		public System.Nullable<bool> IsVegetarian
		{
			get
			{
				return this._IsVegetarian;
			}
			set
			{
				if ((this._IsVegetarian != value))
				{
					this.OnIsVegetarianChanging(value);
					this.SendPropertyChanging();
					this._IsVegetarian = value;
					this.SendPropertyChanged("IsVegetarian");
					this.OnIsVegetarianChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsLowCarb", DbType="Bit")]
		public System.Nullable<bool> IsLowCarb
		{
			get
			{
				return this._IsLowCarb;
			}
			set
			{
				if ((this._IsLowCarb != value))
				{
					this.OnIsLowCarbChanging(value);
					this.SendPropertyChanging();
					this._IsLowCarb = value;
					this.SendPropertyChanged("IsLowCarb");
					this.OnIsLowCarbChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsLowFat", DbType="Bit")]
		public System.Nullable<bool> IsLowFat
		{
			get
			{
				return this._IsLowFat;
			}
			set
			{
				if ((this._IsLowFat != value))
				{
					this.OnIsLowFatChanging(value);
					this.SendPropertyChanging();
					this._IsLowFat = value;
					this.SendPropertyChanged("IsLowFat");
					this.OnIsLowFatChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Ingredient_RecipeIngredient", Storage="_RecipeIngredients", ThisKey="Id", OtherKey="IngredientID")]
		public EntitySet<RecipeIngredient> RecipeIngredients
		{
			get
			{
				return this._RecipeIngredients;
			}
			set
			{
				this._RecipeIngredients.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="IngredientType_Ingredient", Storage="_IngredientType", ThisKey="TypeID", OtherKey="Id", IsForeignKey=true)]
		public IngredientType IngredientType
		{
			get
			{
				return this._IngredientType.Entity;
			}
			set
			{
				IngredientType previousValue = this._IngredientType.Entity;
				if (((previousValue != value) 
							|| (this._IngredientType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._IngredientType.Entity = null;
						previousValue.Ingredients.Remove(this);
					}
					this._IngredientType.Entity = value;
					if ((value != null))
					{
						value.Ingredients.Add(this);
						this._TypeID = value.Id;
					}
					else
					{
						this._TypeID = default(Nullable<long>);
					}
					this.SendPropertyChanged("IngredientType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_RecipeIngredients(RecipeIngredient entity)
		{
			this.SendPropertyChanging();
			entity.Ingredient = this;
		}
		
		private void detach_RecipeIngredients(RecipeIngredient entity)
		{
			this.SendPropertyChanging();
			entity.Ingredient = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Units")]
	public partial class Unit : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Symbol;
		
		private string _Name;
		
		private long _TypeID;
		
		private EntitySet<RecipeIngredient> _RecipeIngredients;
		
		private EntityRef<UnitType> _UnitType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnSymbolChanging(string value);
    partial void OnSymbolChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnTypeIDChanging(long value);
    partial void OnTypeIDChanged();
    #endregion
		
		public Unit()
		{
			this._RecipeIngredients = new EntitySet<RecipeIngredient>(new Action<RecipeIngredient>(this.attach_RecipeIngredients), new Action<RecipeIngredient>(this.detach_RecipeIngredients));
			this._UnitType = default(EntityRef<UnitType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Symbol", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string Symbol
		{
			get
			{
				return this._Symbol;
			}
			set
			{
				if ((this._Symbol != value))
				{
					this.OnSymbolChanging(value);
					this.SendPropertyChanging();
					this._Symbol = value;
					this.SendPropertyChanged("Symbol");
					this.OnSymbolChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(50)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeID", DbType="BigInt NOT NULL")]
		public long TypeID
		{
			get
			{
				return this._TypeID;
			}
			set
			{
				if ((this._TypeID != value))
				{
					if (this._UnitType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnTypeIDChanging(value);
					this.SendPropertyChanging();
					this._TypeID = value;
					this.SendPropertyChanged("TypeID");
					this.OnTypeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Unit_RecipeIngredient", Storage="_RecipeIngredients", ThisKey="Id", OtherKey="UnitID")]
		public EntitySet<RecipeIngredient> RecipeIngredients
		{
			get
			{
				return this._RecipeIngredients;
			}
			set
			{
				this._RecipeIngredients.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="UnitType_Unit", Storage="_UnitType", ThisKey="TypeID", OtherKey="Id", IsForeignKey=true)]
		public UnitType UnitType
		{
			get
			{
				return this._UnitType.Entity;
			}
			set
			{
				UnitType previousValue = this._UnitType.Entity;
				if (((previousValue != value) 
							|| (this._UnitType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._UnitType.Entity = null;
						previousValue.Units.Remove(this);
					}
					this._UnitType.Entity = value;
					if ((value != null))
					{
						value.Units.Add(this);
						this._TypeID = value.Id;
					}
					else
					{
						this._TypeID = default(long);
					}
					this.SendPropertyChanged("UnitType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_RecipeIngredients(RecipeIngredient entity)
		{
			this.SendPropertyChanging();
			entity.Unit = this;
		}
		
		private void detach_RecipeIngredients(RecipeIngredient entity)
		{
			this.SendPropertyChanging();
			entity.Unit = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.IngredientTypes")]
	public partial class IngredientType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private System.Nullable<long> _TargetUnitType;
		
		private EntitySet<Ingredient> _Ingredients;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnTargetUnitTypeChanging(System.Nullable<long> value);
    partial void OnTargetUnitTypeChanged();
    #endregion
		
		public IngredientType()
		{
			this._Ingredients = new EntitySet<Ingredient>(new Action<Ingredient>(this.attach_Ingredients), new Action<Ingredient>(this.detach_Ingredients));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TargetUnitType", DbType="BigInt")]
		public System.Nullable<long> TargetUnitType
		{
			get
			{
				return this._TargetUnitType;
			}
			set
			{
				if ((this._TargetUnitType != value))
				{
					this.OnTargetUnitTypeChanging(value);
					this.SendPropertyChanging();
					this._TargetUnitType = value;
					this.SendPropertyChanged("TargetUnitType");
					this.OnTargetUnitTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="IngredientType_Ingredient", Storage="_Ingredients", ThisKey="Id", OtherKey="TypeID")]
		public EntitySet<Ingredient> Ingredients
		{
			get
			{
				return this._Ingredients;
			}
			set
			{
				this._Ingredients.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Ingredients(Ingredient entity)
		{
			this.SendPropertyChanging();
			entity.IngredientType = this;
		}
		
		private void detach_Ingredients(Ingredient entity)
		{
			this.SendPropertyChanging();
			entity.IngredientType = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.RecipeIngredients")]
	public partial class RecipeIngredient : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private long _RecipeID;
		
		private long _IngredientID;
		
		private decimal _Quantity;
		
		private long _UnitID;
		
		private EntityRef<Ingredient> _Ingredient;
		
		private EntityRef<Unit> _Unit;
		
		private EntityRef<Recipe> _Recipe;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnRecipeIDChanging(long value);
    partial void OnRecipeIDChanged();
    partial void OnIngredientIDChanging(long value);
    partial void OnIngredientIDChanged();
    partial void OnQuantityChanging(decimal value);
    partial void OnQuantityChanged();
    partial void OnUnitIDChanging(long value);
    partial void OnUnitIDChanged();
    #endregion
		
		public RecipeIngredient()
		{
			this._Ingredient = default(EntityRef<Ingredient>);
			this._Unit = default(EntityRef<Unit>);
			this._Recipe = default(EntityRef<Recipe>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecipeID", DbType="BigInt NOT NULL")]
		public long RecipeID
		{
			get
			{
				return this._RecipeID;
			}
			set
			{
				if ((this._RecipeID != value))
				{
					if (this._Recipe.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnRecipeIDChanging(value);
					this.SendPropertyChanging();
					this._RecipeID = value;
					this.SendPropertyChanged("RecipeID");
					this.OnRecipeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IngredientID", DbType="BigInt NOT NULL")]
		public long IngredientID
		{
			get
			{
				return this._IngredientID;
			}
			set
			{
				if ((this._IngredientID != value))
				{
					if (this._Ingredient.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnIngredientIDChanging(value);
					this.SendPropertyChanging();
					this._IngredientID = value;
					this.SendPropertyChanged("IngredientID");
					this.OnIngredientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Quantity", DbType="Decimal(18,2) NOT NULL")]
		public decimal Quantity
		{
			get
			{
				return this._Quantity;
			}
			set
			{
				if ((this._Quantity != value))
				{
					this.OnQuantityChanging(value);
					this.SendPropertyChanging();
					this._Quantity = value;
					this.SendPropertyChanged("Quantity");
					this.OnQuantityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UnitID", DbType="BigInt NOT NULL")]
		public long UnitID
		{
			get
			{
				return this._UnitID;
			}
			set
			{
				if ((this._UnitID != value))
				{
					if (this._Unit.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnUnitIDChanging(value);
					this.SendPropertyChanging();
					this._UnitID = value;
					this.SendPropertyChanged("UnitID");
					this.OnUnitIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Ingredient_RecipeIngredient", Storage="_Ingredient", ThisKey="IngredientID", OtherKey="Id", IsForeignKey=true)]
		public Ingredient Ingredient
		{
			get
			{
				return this._Ingredient.Entity;
			}
			set
			{
				Ingredient previousValue = this._Ingredient.Entity;
				if (((previousValue != value) 
							|| (this._Ingredient.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Ingredient.Entity = null;
						previousValue.RecipeIngredients.Remove(this);
					}
					this._Ingredient.Entity = value;
					if ((value != null))
					{
						value.RecipeIngredients.Add(this);
						this._IngredientID = value.Id;
					}
					else
					{
						this._IngredientID = default(long);
					}
					this.SendPropertyChanged("Ingredient");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Unit_RecipeIngredient", Storage="_Unit", ThisKey="UnitID", OtherKey="Id", IsForeignKey=true)]
		public Unit Unit
		{
			get
			{
				return this._Unit.Entity;
			}
			set
			{
				Unit previousValue = this._Unit.Entity;
				if (((previousValue != value) 
							|| (this._Unit.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Unit.Entity = null;
						previousValue.RecipeIngredients.Remove(this);
					}
					this._Unit.Entity = value;
					if ((value != null))
					{
						value.RecipeIngredients.Add(this);
						this._UnitID = value.Id;
					}
					else
					{
						this._UnitID = default(long);
					}
					this.SendPropertyChanged("Unit");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Recipe_RecipeIngredient", Storage="_Recipe", ThisKey="RecipeID", OtherKey="Id", IsForeignKey=true)]
		public Recipe Recipe
		{
			get
			{
				return this._Recipe.Entity;
			}
			set
			{
				Recipe previousValue = this._Recipe.Entity;
				if (((previousValue != value) 
							|| (this._Recipe.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Recipe.Entity = null;
						previousValue.RecipeIngredients.Remove(this);
					}
					this._Recipe.Entity = value;
					if ((value != null))
					{
						value.RecipeIngredients.Add(this);
						this._RecipeID = value.Id;
					}
					else
					{
						this._RecipeID = default(long);
					}
					this.SendPropertyChanged("Recipe");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Recipes")]
	public partial class Recipe : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private decimal _PortionQuantity;
		
		private string _Source;
		
		private System.Nullable<int> _SourcePage;
		
		private string _SourceISBN;
		
		private string _Summary;
		
		private EntitySet<RecipeIngredient> _RecipeIngredients;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnPortionQuantityChanging(decimal value);
    partial void OnPortionQuantityChanged();
    partial void OnSourceChanging(string value);
    partial void OnSourceChanged();
    partial void OnSourcePageChanging(System.Nullable<int> value);
    partial void OnSourcePageChanged();
    partial void OnSourceISBNChanging(string value);
    partial void OnSourceISBNChanged();
    partial void OnSummaryChanging(string value);
    partial void OnSummaryChanged();
    #endregion
		
		public Recipe()
		{
			this._RecipeIngredients = new EntitySet<RecipeIngredient>(new Action<RecipeIngredient>(this.attach_RecipeIngredients), new Action<RecipeIngredient>(this.detach_RecipeIngredients));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PortionQuantity", DbType="Decimal(18,2) NOT NULL")]
		public decimal PortionQuantity
		{
			get
			{
				return this._PortionQuantity;
			}
			set
			{
				if ((this._PortionQuantity != value))
				{
					this.OnPortionQuantityChanging(value);
					this.SendPropertyChanging();
					this._PortionQuantity = value;
					this.SendPropertyChanged("PortionQuantity");
					this.OnPortionQuantityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Source", DbType="NVarChar(50)")]
		public string Source
		{
			get
			{
				return this._Source;
			}
			set
			{
				if ((this._Source != value))
				{
					this.OnSourceChanging(value);
					this.SendPropertyChanging();
					this._Source = value;
					this.SendPropertyChanged("Source");
					this.OnSourceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SourcePage", DbType="Int")]
		public System.Nullable<int> SourcePage
		{
			get
			{
				return this._SourcePage;
			}
			set
			{
				if ((this._SourcePage != value))
				{
					this.OnSourcePageChanging(value);
					this.SendPropertyChanging();
					this._SourcePage = value;
					this.SendPropertyChanged("SourcePage");
					this.OnSourcePageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SourceISBN", DbType="NChar(10)")]
		public string SourceISBN
		{
			get
			{
				return this._SourceISBN;
			}
			set
			{
				if ((this._SourceISBN != value))
				{
					this.OnSourceISBNChanging(value);
					this.SendPropertyChanging();
					this._SourceISBN = value;
					this.SendPropertyChanged("SourceISBN");
					this.OnSourceISBNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Summary", DbType="NVarChar(50)")]
		public string Summary
		{
			get
			{
				return this._Summary;
			}
			set
			{
				if ((this._Summary != value))
				{
					this.OnSummaryChanging(value);
					this.SendPropertyChanging();
					this._Summary = value;
					this.SendPropertyChanged("Summary");
					this.OnSummaryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Recipe_RecipeIngredient", Storage="_RecipeIngredients", ThisKey="Id", OtherKey="RecipeID")]
		public EntitySet<RecipeIngredient> RecipeIngredients
		{
			get
			{
				return this._RecipeIngredients;
			}
			set
			{
				this._RecipeIngredients.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_RecipeIngredients(RecipeIngredient entity)
		{
			this.SendPropertyChanging();
			entity.Recipe = this;
		}
		
		private void detach_RecipeIngredients(RecipeIngredient entity)
		{
			this.SendPropertyChanging();
			entity.Recipe = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.UnitTypes")]
	public partial class UnitType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private EntitySet<Unit> _Units;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public UnitType()
		{
			this._Units = new EntitySet<Unit>(new Action<Unit>(this.attach_Units), new Action<Unit>(this.detach_Units));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="UnitType_Unit", Storage="_Units", ThisKey="Id", OtherKey="TypeID")]
		public EntitySet<Unit> Units
		{
			get
			{
				return this._Units;
			}
			set
			{
				this._Units.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Units(Unit entity)
		{
			this.SendPropertyChanging();
			entity.UnitType = this;
		}
		
		private void detach_Units(Unit entity)
		{
			this.SendPropertyChanging();
			entity.UnitType = null;
		}
	}
}
#pragma warning restore 1591