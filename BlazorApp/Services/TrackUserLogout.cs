namespace BlazorApp.Services
{
    // Класс для отслеживания выхода текущего юзера из системы.
    public class TrackUserLogout
    {
        // Для отслеживания пользователя используется его зарегистрированное имя,
        // но можно использовать и другие маркеры.
        public delegate void UserLogoutEventHandler(string userName);

        public event UserLogoutEventHandler UserOut;

        // Вызов события выхода юзера из системы.
        // Возбуждается в Areas\Identity\Pages\Account\LogOut.cshtml
        // Выполняется в Shared\MainLayout.razor
        public virtual void OnUserOut(string name) => UserOut?.Invoke(name);
    }
}
