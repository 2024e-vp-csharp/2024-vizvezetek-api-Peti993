using Vizvezetek.API.Models;
using Vizvezetek.API.DTos;

namespace Vizvezetek.API.Helpers
{
    public static class MunkalapMapper
    {
        public static MunkalapDto ToDto(munkalap entity)
        {
            return new MunkalapDto
            {
                Id = entity.id,
                Beadas_datum = entity.beadas_datum.ToDateTime(TimeOnly.MinValue),
                Javitas_datum = entity.javitas_datum.ToDateTime(TimeOnly.MinValue),
                Helyszin = $"{entity.hely.telepules}, {entity.hely.utca}",
                Szerelo = $"{entity.szerelo.nev}",
                Munkaora = entity.munkaora,
                Anyagar = entity.anyagar
            };
        }
    }
}
