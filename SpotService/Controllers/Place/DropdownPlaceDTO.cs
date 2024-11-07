using SpotService.Controllers.Destination;

namespace SpotService.Controllers.Place
{
    public class DropdownPlaceDTO
    {
        public DesKeyValueDTO Destination { get; set; }
        public List<PlaceKeyValueDTO> Places { get; set; }
    }
}
