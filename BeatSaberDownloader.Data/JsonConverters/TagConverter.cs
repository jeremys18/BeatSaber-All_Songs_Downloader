
using BeatSaberDownloader.Data.Models.DetailedModels;
using Newtonsoft.Json;

namespace BeatSaberDownloader.Data.JsonConverters
{
    public class TagConverter : JsonConverter<Tag>
    {
        public override Tag? ReadJson(JsonReader reader, Type objectType, Tag? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String || reader.Value ==  null) return null;
            var s = reader.Value.ToString();
            return new Tag
            {
                Id = 0,
                Name = s
            };
        }

        public override void WriteJson(JsonWriter writer, Tag? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
