using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZQNB.Common.Data.Model
{
    public interface INbEntity<TPk>
    {
        TPk Id { get; set; }
    }
    
    public abstract class NbEntityBase<T, TPk> : INbEntity<TPk> where T : class, INbEntity<TPk>
    {
        public virtual TPk Id { get; set; }

        private readonly TPk _defaultIdValue = default(TPk);
        private int? _oldHashCode;

        public override int GetHashCode()
        {
            // once we have a hashcode we'll never change it
            if (_oldHashCode.HasValue)
                return _oldHashCode.Value;
            // when this instance is new we use the base hash code
            // and remember it, so an instance can NEVER change its
            // hash code.
            var thisIsNew = Equals(Id, _defaultIdValue);
            if (thisIsNew)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as T;
            if (other == null) return false;

            var thisIsNew = Equals(Id, _defaultIdValue);
            var otherIsNew = Equals(other.Id, _defaultIdValue);

            if (thisIsNew && otherIsNew)
                return ReferenceEquals(this, other);

            return Id.Equals(other.Id);
        }

        public static bool operator ==(NbEntityBase<T, TPk> lhs, NbEntityBase<T, TPk> rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(NbEntityBase<T, TPk> lhs, NbEntityBase<T, TPk> rhs)
        {
            return !Equals(lhs, rhs);
        }
    }

    //aggregate root entity
    public abstract class NbEntity<T> : NbEntityBase<T, Guid> where T : class ,INbEntity<Guid>
    {
    }

    //not root entity
    public abstract class NbEntityInt<T> : NbEntityBase<T, int> where T : NbEntityInt<T>
    {
    }

    //value object
    public abstract class NbValueObject<T> : IEquatable<T> where T : NbValueObject<T>
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            T other = obj as T;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            IEnumerable<FieldInfo> fields = GetFields();

            int startValue = 17;
            int multiplier = 59;

            int hashCode = startValue;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;

            Type t = GetType();
            Type otherType = other.GetType();

            if (t != otherType)
                return false;

            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                object value1 = field.GetValue(other);
                object value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            Type t = GetType();

            List<FieldInfo> fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

                t = t.BaseType;
            }

            return fields;
        }

        public static bool operator ==(NbValueObject<T> x, NbValueObject<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(NbValueObject<T> x, NbValueObject<T> y)
        {
            return !(x == y);
        }
    }
}
