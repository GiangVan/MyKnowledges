using System.Management;

namespace YourExperience
{
	class ShowComputer
	{
		private static string key = "SELECT * FROM ";
		private static string data = null;
		private static ManagementObjectSearcher searcher;
		public static string Show()
		{
			Registry();
			CPU();
			RAM();
			Display();
			Graphic();
			Board();
			Disk();
			return data;
		}

		private static void Registry()
		{
			data += "Registry";
			using (searcher = new ManagementObjectSearcher(key + "Win32_Registry"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "Name" || Property.Name == "Status")
							{
								data += "\r\n\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									try
									{
										data += Property.Value.ToString().Substring(0, Property.Value.ToString().IndexOf('|'));
									}
									catch
									{
										data += Property.Value.ToString();
									}
								}
								else data += "null";
							}
							else if (Property.Name == "InstallDate")
							{
								data += "\r\n\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									try
									{
										string date = Property.Value.ToString().Substring(0, 14);
										data += date.Substring(8, 2) + " " + date.Substring(10, 2) + " " + date.Substring(12, 2) + "  " + date.Substring(6, 2) + "/" + date.Substring(4, 2) + "/" + date.Substring(0, 4);
									}
									catch
									{
										data += Property.Value.ToString();
									}
								}
								else data += "null";
							}
						}
					}
				}
				catch { }				
		}

		private static string FormatNumber(ulong value)                                                                            //thêm dấu chấm vào số.
		{
			string stringINPUT = value.ToString();
			if (value < 0) stringINPUT.Remove(0, 1);
			int n = (stringINPUT.Length + 2) / 3;
			for (int i = 1; i < n; i++)
			{
				stringINPUT = stringINPUT.Insert(stringINPUT.Length - (i * 3 + i - 1), ".");
			}
			if (value < 0) stringINPUT.Insert(0, "-");
			return stringINPUT;
		}

		private static void Board()
		{
			data += "\r\n\r\nBaseBoard";
			using (searcher = new ManagementObjectSearcher(key + "Win32_BaseBoard"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "Product" || Property.Name == "Status" || Property.Name == "Manufacturer" || Property.Name == "Version" || Property.Name == "Removable" || Property.Name == "Replaceable" || Property.Name == "RequiresDaughterBoard" || Property.Name == "HostingBoard")
							{
								data += "\r\n\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString();
								}
								else data += "null";
							}
						}
					}
				}
				catch { }
		}

		private static void CPU()
		{
			data += "\r\n\r\nCPU";
			using (searcher = new ManagementObjectSearcher(key + "Win32_Processor"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "Name" || Property.Name == "NumberOfCores" || Property.Name == "ThreadCount" || Property.Name == "UpgradeMethod" || Property.Name == "VirtualizationFirmwareEnabled" || Property.Name == "DataWidth")
							{
								data += "\r\n\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString();
								}
								else data += "null";
							}
						}
					}
				}
				catch { }
			using (searcher = new ManagementObjectSearcher(key + "Win32_CacheMemory"))
				try
				{
					string value = null;
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "MaxCacheSize")
							{
								if (Property.Value != null)
								{
									value = Property.Value.ToString() + "(KB)";
								}
								else value = "null";
							}
							if (Property.Name == "Purpose")
							{
								data += "\r\n\t" + Property.Value + ": " + value;
							}
						}
					}
				}
				catch { }
		}

		private static void RAM()
		{
			data += "\r\n\r\nRAM";
			using (searcher = new ManagementObjectSearcher(key + "Win32_PhysicalMemory"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if(Property.Name == "BankLabel")
							{
								if (Property.Value != null)
								{
									data += "\r\n\t" + Property.Value.ToString() + ":";
								}
								else data += "\r\n\t";
							}
							else if (Property.Name == "PartNumber" || Property.Name == "DataWidth")
							{
								data += "\r\n\t\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString();
								}
								else data += "null";
							}
							else if(Property.Name == "Speed")
							{
								data += "\r\n\t\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString() + "(MHz)";
								}
								else data += "null";
							}
							else if (Property.Name == "Capacity")
							{
								data += "\r\n\t\tSize: ";
								if (Property.Value != null)
								{
									try
									{
										data += (((ulong)Property.Value/1024)/1024).ToString() + "(MB)";
									}
									catch
									{
										data += Property.Value.ToString() + "(Bytes)";
									}
								}
								else data += "null";
							}
						}
					}
				}
				catch { }
		}

		private static void Display()
		{/*
			data += "\r\n\r\nDisplay";
			using (searcher = new ManagementObjectSearcher(key + "Win32_DisplayConfiguration"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "DeviceName" || Property.Name == "BitsPerPel")
							{
								data += "\r\n\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString();
								}
								else data += "null";
							}
							else if(Property.Name == "DisplayFrequency")
							{
								data += "\r\n\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString() + "(Hz)";
								}
								else data += "null";
							}
						}
					}
				}
				catch { }*/
		}

		private static void Graphic()
		{
			data += "\r\n\r\nGraphic";
			using (searcher = new ManagementObjectSearcher(key + "Win32_VideoController"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "AdapterCompatibility")
							{
								if (Property.Value != null)
								{
									data += "\r\n\t" + Property.Value.ToString() + ":";
								}
								else data += "\r\n\t";
							}
							else if (Property.Name == "VideoModeDescription" || Property.Name == "Name" || Property.Name == "DriverVersion" || Property.Name == "InstalledDisplayDrivers" || Property.Name == "VideoProcessor" || Property.Name == "AdapterDACType" || Property.Name == "CurrentBitsPerPixel" || Property.Name == "Status")
							{
								data += "\r\n\t\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString();
								}
								else data += "null";
							}
							else if(Property.Name == "CurrentRefreshRate")
							{
								data += "\r\n\t\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString() + "(Hz)";
								}
								else data += "null";
							}
							else if (Property.Name == "AdapterRAM")
							{
								data += "\r\n\t\tVRAM: ";
								if (Property.Value != null)
								{
									try
									{
										data += (((uint)Property.Value / 1024) / 1024).ToString() + "(MB)";
									}
									catch
									{
										data += Property.Value.ToString() + "(Bytes)";
									}
								}
								else data += "null";
							}
						}
					}
				}
				catch { }
		}

		private static void Disk()
		{
			data += "\r\n\r\nDisk";
			using (searcher = new ManagementObjectSearcher(key + "Win32_DiskDrive"))
				try
				{
					foreach (ManagementObject managementObject in searcher.Get())
					{
						foreach (PropertyData Property in managementObject.Properties)
						{
							if (Property.Name == "Caption")
							{
								if (Property.Value != null)
								{
									data += "\r\n\t" + Property.Value.ToString();
								}
								else data += "\r\n\t";
							}
							else if(Property.Name == "TotalSectors" || Property.Name == "TotalTracks" || Property.Name == "TotalHeads" || Property.Name == "TotalCylinders")
							{
								data += "\r\n\t\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									try
									{
										data += FormatNumber((ulong)Property.Value);
									}
									catch
									{
										data += Property.Value.ToString();
									}									
								}
								else data += "null";
							}
							else if (Property.Name == "Index" || Property.Name == "FirmwareRevision" || Property.Name == "Status")
							{
								data += "\r\n\t\t" + Property.Name + ": ";
								if (Property.Value != null)
								{
									data += Property.Value.ToString();
								}
								else data += "null";
							}
							else if (Property.Name == "Size")
							{
								data += "\r\n\t\tSize: ";
								if (Property.Value != null)
								{
									try
									{
										if((ulong)Property.Value > 1047527424)
											data += ((((ulong)Property.Value / 1024) / 1024) / 1024).ToString() + "(GB)";
										else
											data += (((ulong)Property.Value / 1024) / 1024).ToString() + "(MB)";
									}
									catch
									{
										data += Property.Value.ToString() + "(Bytes)";
									}
								}
								else data += "null";
							}
						}
					}
				}
				catch { }
		}
	}
}
