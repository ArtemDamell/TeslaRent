﻿using AutoMapper;
using DataAccess.Data;
using Models;
using Models.DTO;

namespace Business.Mapper
{
    // 21. Создаём класс профиля для AutoMapper
    /*
        Чтобы превратить обычный класс в профиль автомаппера и для того,
        Чтобы он унаследовал весь необходимый функционал, мы должны унаследовать
        Наш класс от базового Profile
     */
    public class MappingProfile : Profile
    {
        // 21.1 Создаём конструктор класса для начальной настройки профиля
        public MappingProfile()
        {
            // 21.2 Создаём метод сопастовления 2-х объектов
            /*
                Этот метод позволяет сопаставить 2 модели и искать в них
                Одинаковые имена свойств. Если таковые будут найдены, то в них
                Автоматически занесутся данные из переданной в первом параметре
                Модели данные. Если же имена не будут соответствовать, то в те 
                Свойства, которые небыли найдены занесётся null
             */
            CreateMap<TeslaCarDTO, TeslaCar>();
            // --> Возвращаемся в TeslaCarRepository

            // 25. Добавляем маршрут обратной конвертации
            CreateMap<TeslaCar, TeslaCarDTO>();

            // 55. Добавляем новый маршрут для изображений
            // ReverseMap() позволяет в автоматическом режиме делать обратную конвертацию
            // Этот метод работает только при полном совпадении названий свойств модели
            CreateMap<TeslaCarImage, TeslaCarImageDTO>().ReverseMap();

            // 86. Выполнение домашнего задания
            CreateMap<CarAccessory, CarAccessoryDTO>().ReverseMap();

            // 183.1 После имплементации методов, добавляем карту для автомаппера в MappingProfile 
            //CreateMap<CarOrderDetails, CarOrderDetailsDTO>().ReverseMap();

            // 262. На данном этапе у нас заполнена вся таблица, кроме машины, это происходит из-за того, что автомаппер не конвертирует автоматически данные.Исправить AutoMapperProfile
            CreateMap<CarOrderDetails, CarOrderDetailsDTO>().ForMember(x => x.TeslaCarDTO, options => options.MapFrom(x => x.TeslaCar));
            CreateMap<CarOrderDetailsDTO, CarOrderDetails>();
        }
    }
}
