namespace Domain.SeedWork
{
    /// <summary>
    /// Abstract base class for value objects in a domain-driven design context.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Gets the components that define equality for this value object.
        /// Must be implemented in derived classes to define which properties
        /// determine if two objects are considered equal.
        /// </summary>
        /// <returns>A sequence of objects representing the relevant properties for comparison.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// Compares the equality components element by element.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) // Checks if the object is null OR of a different type
            {
                return false;
            }

            var valueObject = (ValueObject)obj; // Casts the object to ValueObject (we already know it's the correct type)
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents()); // Compares equality components element by element
        }

        /// <summary>
        /// Calculates and returns the hash code for this value object.
        /// Combines the hash codes of all equality components using XOR.
        /// Ensures that equal objects have the same hash code.
        /// </summary>
        /// <returns>A hash code calculated from the equality components.</returns>
        public override int GetHashCode()
        {
            return GetEqualityComponents() // Gets all components
                .Select(obj => obj?.GetHashCode() ?? 0) // Calculates the hash of each component (or 0 if null)
                .Aggregate((x, y) => x ^ y); // Combines all hashes using XOR
        }

        /// <summary>
        /// Equality operator that compares two value objects.
        /// Checks for null references and delegates to the Equals method.
        /// </summary>
        /// <param name="left">The first value object to compare.</param>
        /// <param name="right">The second value object to compare.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) // Both are null = equal
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) // One is null = not equal
                return false;

            return left.Equals(right); // Uses the Equals() method we implemented above
        }

        /// <summary>
        /// Inequality operator that compares two value objects.
        /// Returns the inverse of the equality operator.
        /// </summary>
        /// <param name="left">The first value object to compare.</param>
        /// <param name="right">The second value object to compare.</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right); // Simply negates the result of the == operator
        }
        
        /// <summary>
        /// Creates a copy of the value object.
        /// Since value objects are immutable, it returns the same instance.
        /// Derived classes can override this to implement custom copy logic.
        /// </summary>
        /// <returns>A reference to the same object (since it is immutable).</returns>
        public virtual ValueObject Copy()
        {
            return this; // No need to create a real copy; if a "real" copy is needed, derived classes can override
        }
    }
}
