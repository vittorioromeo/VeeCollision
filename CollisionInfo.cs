namespace VeeCollision
{
    public struct CollisionInfo
    {
        public float Frametime;
        public object UserData;
        public Body Body;

        public CollisionInfo(float mFrameTime, object mUserData, Body mBody)
        {
            Frametime = mFrameTime;
            UserData = mUserData;
            Body = mBody;
        }
    }
}