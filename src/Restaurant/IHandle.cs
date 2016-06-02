﻿namespace Restaurant
{
    public interface IHandle<in TMessage>
    {
        void Handle(TMessage message);
    }
}
