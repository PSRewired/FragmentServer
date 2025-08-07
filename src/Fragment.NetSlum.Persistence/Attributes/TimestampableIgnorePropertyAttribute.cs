using System;

namespace Fragment.NetSlum.Persistence.Attributes;

/// <summary>
/// Denotes that certain properties should be ignored when updating a <see cref="TimestampableAttribute"/>
/// property.
/// <br/>
/// If the attribute is set on an Entity definition all members containing <see cref="TimestampableAttribute"/>
/// will ignore the specified properties.
/// <br/>
/// <br/>
/// <i>Setting this attribute on a property that does not already contain <see cref="TimestampableAttribute"/> is a no-op.</i>
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
public class TimestampableIgnorePropertyAttribute : Attribute
{
    public readonly string[] PropertyNames;

    public TimestampableIgnorePropertyAttribute(params string[] propertyNames)
    {
        PropertyNames = propertyNames;
    }
}
