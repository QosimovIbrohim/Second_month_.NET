

using System.Text.Json;

namespace TelegramBot
{
    public static class CRUD
    {
        public static string filePath = @"C:\Users\user\Desktop\Users.json";

        public static void Create(User chat)
        {
            List<User> chats = GetAllChats();
            if (chats.Any(c => c.chatID == chat.chatID))
            {
                return;
            }
            chats.Add(chat);
            SaveChats(chats);
        }


        public static string Read(long chatId)
        {
            List<User> chats = GetAllChats();
            var chat = chats.Find(c => c.chatID == chatId);

            if (chat != null)
            {
                return $"{chat.chatID}:{chat.phoneNumber}";
            }
            else
            {
                return $"{chat.chatID}:{chat.phoneNumber}";
            }
        }
        public static bool IsPhoneNumberNull(long chatId)
        {
            List<User> chats = GetAllChats();
            var chat = chats.Find(c => c.chatID == chatId);

            if (chat != null && chat.phoneNumber != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void Update(long chatId, string newPhoneNumber)
        {
            List<User> chats = GetAllChats();
            var chatToUpdate = chats.Find(c => c.chatID == chatId);

            if (chatToUpdate != null)
            {
                chatToUpdate.phoneNumber = newPhoneNumber;
                SaveChats(chats);
            }
        }

        public static void Delete(long chatId)
        {
            List<User> chats = GetAllChats();
            var chatToRemove = chats.Find(c => c.chatID == chatId);

            if (chatToRemove != null)
            {
                chats.Remove(chatToRemove);
                SaveChats(chats);
            }
        }

        private static List<User> GetAllChats()
        {
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                return new List<User>();
            }
        }

        public static List<User> GetAll()
        {
            return GetAllChats();
        }

        private static void SaveChats(List<User> chats)
        {
            string json = JsonSerializer.Serialize(chats);
            System.IO.File.WriteAllText(filePath, json);
        }
    }
    public class User
    {
        public long chatID {  get; set; }
        public string? phoneNumber { get; set; }
    }
}
