using Toast.Common.Configurations;

namespace Toast.Tests.Common.Configurations
{
    public class ToastTestSettings<T, TE> : ToastSettings<T>
    {
        public virtual TE Examples { get; set; }
    }
}