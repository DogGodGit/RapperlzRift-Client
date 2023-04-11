//
//  NativePlugin.m
//  NativePluginTest
//
//  Created by galalab on 2015. 10. 22..
//
//

#import "ApiClient.h"

#define UnityStringFromNSString( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL
#define NSStringFromUnityString( _x_ ) ( _x_ != NULL ) ? [[NSString alloc] initWithCString:_x_ encoding:NSUTF8StringEncoding] : nil

extern "C"
{
    int64_t apiClient(const char *content, const char *objectName, const char *methodName)
    {
        NSString *sContent = [NSString stringWithUTF8String:content];
        NSString *sObjectName = [NSString stringWithUTF8String:objectName];
        NSString *sMethodName = [NSString stringWithUTF8String:methodName];
        
        return [[ApiClient sharedInstance] Request:sContent objectName:sObjectName methodName:sMethodName];
    }
}

@implementation ApiClient

+ (ApiClient*) sharedInstance
{
    static ApiClient* instance;
    if(instance == NULL)
    {
        instance = [[ApiClient alloc] init];
        
        // AsyncToken init
        instance.lnAsyncToken = 0;
        
        [[ILoveGameSDK getInstance] setSplashWithDeveloper:YES];
    }
    return instance;
}

- (id)init
{
    self = [super init];
    if (self) {
        self.onPauseByPass = NO;
    }
    return self;
} 

- (void)initSDK:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    self.application = application;
    
    [[EntermateProxy sharedInstance] initSDK:application didFinishLaunchingWithOptions:launchOptions];
}

- (void)initDelegate:(id)application openURL:(id)url sourceApplication:(id)sourceApplication annotation:(id)annotation
{
    [[EntermateProxy sharedInstance] initDelegate:application openURL:url sourceApplication:sourceApplication annotation:annotation];
}

- (long)GenerateAsyncToken
{
    self.lnAsyncToken = self.lnAsyncToken + 1;
    
    return self.lnAsyncToken;
}

- (int64_t)Request:(NSString *)sContent objectName:(NSString *)objectName methodName:(NSString *)methodName
{
    // 프로토콜 체크
    
    NSDictionary *jsonParam = [JsonUtil deserializeObjectFromJSON:[sContent dataUsingEncoding:NSUTF8StringEncoding]];
    
    if (jsonParam == nil) {
        [NSException raise:NSInvalidArgumentException format:@"프로토콜 오류입니다."];
    }
    
    NSString *sCommand = [jsonParam objectForKey:@"cmd"];
    
    NSLog(@"Command : %@", sCommand);
    
    ApiClientRequestType reqType = ApiClientRequestTypeFromNSString(sCommand);
    
    // Command 프로퍼티 체크
    
    if (reqType == ApiClientRequestTypeUnKnown) {
        [NSException raise:NSInvalidArgumentException format:@"'cmd' 프로퍼티가 유효하지 않습니다."];
    }
    
    int64_t asyncToken = [self GenerateAsyncToken];
    
    switch(reqType)
    {
        case ApiClientRequestTypeLoginWithEntermate : [[EntermateProxy sharedInstance] login:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeCoupon : [[EntermateProxy sharedInstance] coupon:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeLinking : [[EntermateProxy sharedInstance] linking:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeOpenEvent : [[EntermateProxy sharedInstance] openEvent:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeOpenHelp : [[EntermateProxy sharedInstance] openHelp:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeOpenHomepage : [[EntermateProxy sharedInstance] openHomepage:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeOpenIntro : [[EntermateProxy sharedInstance] openIntro:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypePlayerInfo : [[EntermateProxy sharedInstance] playerInfo:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeGetPush : [[EntermateProxy sharedInstance] getPush:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeTogglePush : [[EntermateProxy sharedInstance] togglePush:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeUnRegister : [[EntermateProxy sharedInstance] unRegister:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeAchievementUnlock : [[EntermateProxy sharedInstance] achievementUnlock:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeSendTracking : [[EntermateProxy sharedInstance] sendTracking:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeCharge : [[EntermateProxy sharedInstance] charge:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeIsVisibleFriend : [[EntermateProxy sharedInstance] isVisibleFriend:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeServerList : [[EntermateProxy sharedInstance] serverList:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeHealthCheckServer : [[EntermateProxy sharedInstance] healthCheckServer:sContent asyncToken:asyncToken objectName:objectName methodName:methodName];
            break;
        case ApiClientRequestTypeUnKnown :
            break;
    }
    
//    if ([sCommand isEqual:@"LoginWithFacebook"])
//    {
//        NSLog(@"페북 로그인");
//        
//        [[FacebookProxy sharedInstance] login:sContent asyncToken:(long long)asyncToken objectName:(NSString *)objectName methodName:(NSString *)methodName];
//    }
//    else if([sCommand isEqual:@"LoginWithGoogle"])
//    {
//        NSLog(@"구글 로그인");
//        
//        [[GoogleProxy sharedInstance] login:sContent asyncToken:(long long)asyncToken objectName:(NSString *)objectName methodName:(NSString *)methodName];
//    }
    NSLog(@"asyncToken : %lld", asyncToken);
    return asyncToken;
}

- (void)unitySendMessage:(NSString *)objectName methodName:(NSString *)methodName response:(NSString *)response
{
    NSLog(@"invokeCallbackOnUnityWithObjectAndMethodName:%@, %@, %@", objectName, methodName, response);
    
    UnitySendMessage(UnityStringFromNSString(objectName), UnityStringFromNSString(methodName), UnityStringFromNSString(response));
}

# pragma mark - enum
ApiClientRequestType ApiClientRequestTypeFromNSString(NSString *requestType)
{
    ApiClientRequestType reqType = ApiClientRequestTypeUnKnown;
    
    if([requestType isEqual:@"LoginWithEntermate"])
        reqType = ApiClientRequestTypeLoginWithEntermate;
    
    if([requestType isEqual:@"Coupon"])
        reqType = ApiClientRequestTypeCoupon;
    
    if([requestType isEqual:@"Linking"])
        reqType = ApiClientRequestTypeLinking;
    
    if([requestType isEqual:@"OpenEvent"])
        reqType = ApiClientRequestTypeOpenEvent;
    
    if([requestType isEqual:@"OpenHelp"])
        reqType = ApiClientRequestTypeOpenHelp;
    
    if([requestType isEqual:@"OpenHomepage"])
        reqType = ApiClientRequestTypeOpenHomepage;
    
    if([requestType isEqual:@"OpenIntro"])
        reqType = ApiClientRequestTypeOpenIntro;
    
    if([requestType isEqual:@"PlayerInfo"])
        reqType = ApiClientRequestTypePlayerInfo;
    
    if([requestType isEqual:@"GetPush"])
        reqType = ApiClientRequestTypeGetPush;
    
    if([requestType isEqual:@"TogglePush"])
        reqType = ApiClientRequestTypeTogglePush;
    
    if([requestType isEqual:@"UnRegister"])
        reqType = ApiClientRequestTypeUnRegister;
    
    if([requestType isEqual:@"AchievementUnlock"])
        reqType = ApiClientRequestTypeAchievementUnlock;
    
    if([requestType isEqual:@"SendTracking"])
        reqType = ApiClientRequestTypeSendTracking;
    
    if([requestType isEqual:@"Charge"])
        reqType = ApiClientRequestTypeCharge;
    
    if([requestType isEqual:@"IsVisibleFriend"])
        reqType = ApiClientRequestTypeIsVisibleFriend;
    
    if([requestType isEqual:@"ServerList"])
        reqType = ApiClientRequestTypeServerList;
    
    if([requestType isEqual:@"HealthCheckServer"])
        reqType = ApiClientRequestTypeHealthCheckServer;
    
    return reqType;
}

@end
