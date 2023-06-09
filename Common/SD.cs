﻿namespace Common
{
    // 112. Создаём статический класс для констант
    public static class SD
    {
        public const string ADMIN_ROLE = "Admin";
        public const string CUSTOMER_ROLE = "Customer";
        public const string EMPLOYEE_ROLE = "Employee";

        // 161. Добавить константу в класс SD для хранения ключа LocalStorage
        public const string LOCAL_INITIALBOOKING = "InitialBookingInfo";

        public const string LOCAL_CAR_ORDER_DETAILS = "InitialCarBookingInfo";
        public const string LOCAL_TOKEN = "JWT Token";

        // 229.2 Добавить константу для информации о пользователе
        public const string LOCAL_USER_DETAILS = "User Details";
    }
}
