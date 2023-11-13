namespace NEU_Restaurant.Library.IServices;

public interface IPreferenceStorage
{
	void Set(string key, int value);

	int Get(string key, int defaultValue);

}