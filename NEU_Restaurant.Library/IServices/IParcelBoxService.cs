namespace NEU_Restaurant.Library.IServices;

public interface IParcelBoxService
{
	string Put(object o);
	object Get(string ticket);
}