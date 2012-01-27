#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17020
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wonga.QA.Framework.Db.IpLookup
{
	using Wonga.QA.Framework.Db;
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="IpLookup")]
	public partial class IpLookupDatabase : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertIp2LocationEntity(Ip2LocationEntity instance);
    partial void UpdateIp2LocationEntity(Ip2LocationEntity instance);
    partial void DeleteIp2LocationEntity(Ip2LocationEntity instance);
    partial void InsertMSSQLDeploy(MSSQLDeploy instance);
    partial void UpdateMSSQLDeploy(MSSQLDeploy instance);
    partial void DeleteMSSQLDeploy(MSSQLDeploy instance);
    #endregion
		
		public IpLookupDatabase(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public IpLookupDatabase(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public IpLookupDatabase(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public IpLookupDatabase(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Ip2LocationEntity> Ip2Locations
		{
			get
			{
				return this.GetTable<Ip2LocationEntity>().SetTable<Ip2LocationEntity>();
			}
		}
		
		public System.Data.Linq.Table<MSSQLDeploy> MSSQLDeploys
		{
			get
			{
				return this.GetTable<MSSQLDeploy>().SetTable<MSSQLDeploy>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="lookup.Ip2Location")]
	public partial class Ip2LocationEntity : DbEntity<Ip2LocationEntity>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Ip2LocationId;
		
		private double _IpFrom;
		
		private double _IpTo;
		
		private string _CountryShort;
		
		private string _Country;
		
		private string _Region;
		
		private string _City;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIp2LocationIdChanging(int value);
    partial void OnIp2LocationIdChanged();
    partial void OnIpFromChanging(double value);
    partial void OnIpFromChanged();
    partial void OnIpToChanging(double value);
    partial void OnIpToChanged();
    partial void OnCountryShortChanging(string value);
    partial void OnCountryShortChanged();
    partial void OnCountryChanging(string value);
    partial void OnCountryChanged();
    partial void OnRegionChanging(string value);
    partial void OnRegionChanged();
    partial void OnCityChanging(string value);
    partial void OnCityChanged();
    #endregion
		
		public Ip2LocationEntity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Ip2LocationId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Ip2LocationId
		{
			get
			{
				return this._Ip2LocationId;
			}
			set
			{
				if ((this._Ip2LocationId != value))
				{
					this.OnIp2LocationIdChanging(value);
					this.SendPropertyChanging();
					this._Ip2LocationId = value;
					this.SendPropertyChanged("Ip2LocationId");
					this.OnIp2LocationIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IpFrom", DbType="Float NOT NULL")]
		public double IpFrom
		{
			get
			{
				return this._IpFrom;
			}
			set
			{
				if ((this._IpFrom != value))
				{
					this.OnIpFromChanging(value);
					this.SendPropertyChanging();
					this._IpFrom = value;
					this.SendPropertyChanged("IpFrom");
					this.OnIpFromChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IpTo", DbType="Float NOT NULL")]
		public double IpTo
		{
			get
			{
				return this._IpTo;
			}
			set
			{
				if ((this._IpTo != value))
				{
					this.OnIpToChanging(value);
					this.SendPropertyChanging();
					this._IpTo = value;
					this.SendPropertyChanged("IpTo");
					this.OnIpToChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CountryShort", DbType="NVarChar(2)")]
		public string CountryShort
		{
			get
			{
				return this._CountryShort;
			}
			set
			{
				if ((this._CountryShort != value))
				{
					this.OnCountryShortChanging(value);
					this.SendPropertyChanging();
					this._CountryShort = value;
					this.SendPropertyChanged("CountryShort");
					this.OnCountryShortChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Country", DbType="NVarChar(64)")]
		public string Country
		{
			get
			{
				return this._Country;
			}
			set
			{
				if ((this._Country != value))
				{
					this.OnCountryChanging(value);
					this.SendPropertyChanging();
					this._Country = value;
					this.SendPropertyChanged("Country");
					this.OnCountryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Region", DbType="NVarChar(128)")]
		public string Region
		{
			get
			{
				return this._Region;
			}
			set
			{
				if ((this._Region != value))
				{
					this.OnRegionChanging(value);
					this.SendPropertyChanging();
					this._Region = value;
					this.SendPropertyChanged("Region");
					this.OnRegionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_City", DbType="NVarChar(128)")]
		public string City
		{
			get
			{
				return this._City;
			}
			set
			{
				if ((this._City != value))
				{
					this.OnCityChanging(value);
					this.SendPropertyChanging();
					this._City = value;
					this.SendPropertyChanged("City");
					this.OnCityChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MSSQLDeploy")]
	public partial class MSSQLDeploy : DbEntity<MSSQLDeploy>, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private System.DateTime _Date;
		
		private string _Name;
		
		private string _MD5;
		
		private System.Nullable<int> _Revision;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnMD5Changing(string value);
    partial void OnMD5Changed();
    partial void OnRevisionChanging(System.Nullable<int> value);
    partial void OnRevisionChanged();
    #endregion
		
		public MSSQLDeploy()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MD5", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string MD5
		{
			get
			{
				return this._MD5;
			}
			set
			{
				if ((this._MD5 != value))
				{
					this.OnMD5Changing(value);
					this.SendPropertyChanging();
					this._MD5 = value;
					this.SendPropertyChanged("MD5");
					this.OnMD5Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Revision", DbType="Int")]
		public System.Nullable<int> Revision
		{
			get
			{
				return this._Revision;
			}
			set
			{
				if ((this._Revision != value))
				{
					this.OnRevisionChanging(value);
					this.SendPropertyChanging();
					this._Revision = value;
					this.SendPropertyChanged("Revision");
					this.OnRevisionChanged();
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
}
#pragma warning restore 1591
