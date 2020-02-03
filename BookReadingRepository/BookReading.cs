using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BookReadingProject
{
    public class BookReading : IEquatable<BookReading>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id;
        
        public string name = string.Empty;
        public int priority = 0;

        public bool Equals(BookReading other)
        {
            return id.Equals(other.id) && name.Equals(other.name) && priority.Equals(other.priority);
        }
    }
}
