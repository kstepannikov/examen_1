using System.Text;

namespace BuildingKirill
{
	static public class LoginUtility
	{

		static public Сотрудники сотрудник = null;

		static public string GetHash(string pass)
		{
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
				byte[] inputBytes = Encoding.ASCII.GetBytes(pass);
				byte[] hashBytes = md5.ComputeHash(inputBytes);


				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("X2"));
				}

				return sb.ToString();
			}
		}
		static public string GetInfo()
		{
			return сотрудник.Фамилия + " " + сотрудник.Имя + " " + сотрудник.Отчество + " (" + сотрудник.Должность.Наименование + ")";
		}


	}

}
