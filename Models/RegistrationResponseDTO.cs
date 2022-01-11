using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    // 129.2 Теперь нам потребуется модель DTO для ответа сервера пользователю
    public class RegistrationResponseDTO
    {
        public bool IsRegistrationSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
