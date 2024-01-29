

using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

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

            try
            {
                List<User> users = GetAllChats();

                if (users != null)
                {
                    int index = users.FindIndex(u => u.chatID == chatId);


                    if (index != -1)
                    {
                        users[index].phoneNumber = newPhoneNumber;

                        SaveChats(users);
                    }
                }
            }
            catch
            {

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
        public static int GetStatusCode(long chatId)
        {

            List<User> users = GetAllChats();
            User? chatToRemove = users.Find(c => c.chatID == chatId);

            return chatToRemove.status;
        }

        public static void ChangeStatusCode(long chatId,int statusCode)
        {

            List<User> users = GetAllChats();
            int index = users.FindIndex(u => u.chatID == chatId);
            if(index != -1)
            {
                users[index].status = statusCode;
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
        public long chatID { get; set; }

        public int status { get; set; }
        public string? phoneNumber { get; set; }
    }
}
