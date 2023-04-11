//
//  NativePlugin.h
//  NativePluginTest
//
//  Created by galalab on 2015. 10. 22..
//
//

#import <Foundation/Foundation.h>
//#import "FacebookProxy.h"
//#import "GoogleProxy.h"
#import "EntermateProxy.h"
#import "LogUtil.h"
#import "JsonUtil.h"

static const int kResult_OK             = 0;
static const int kResult_Error          = 1;
static const int kResult_Canceled       = 101;
static const int kResult_LinkMoved      = 101;
static const int kResult_LinkCancled    = 102;

@interface ApiClient : NSObject

@property (nonatomic, assign) int64_t lnAsyncToken;
@property (nonatomic, strong) NSString *googleClientId;
@property (nonatomic, strong) UIApplication *application;
@property (nonatomic) BOOL onPauseByPass;

+ (ApiClient*) sharedInstance;

#pragma mark - Init
- (void)initSDK:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions;
- (void)initDelegate:application openURL:url sourceApplication:sourceApplication annotation:annotation;

#pragma mark - Request

- (int64_t)Request:(NSString *)sContent objectName:(NSString *)objectName methodName:(NSString *)methodName;
- (void)unitySendMessage:(NSString *)objectName methodName:(NSString *)methodName response:(NSString *)response;

typedef enum _ApiClientRequestType{
    ApiClientRequestTypeUnKnown = 0,
    ApiClientRequestTypeLoginWithEntermate,
    ApiClientRequestTypeCoupon,
    ApiClientRequestTypeLinking,
    ApiClientRequestTypeOpenEvent,
    ApiClientRequestTypeOpenHelp,
    ApiClientRequestTypeOpenHomepage,
    ApiClientRequestTypeOpenIntro,
    ApiClientRequestTypePlayerInfo,
    ApiClientRequestTypeGetPush,
    ApiClientRequestTypeTogglePush,
    ApiClientRequestTypeUnRegister,
    ApiClientRequestTypeAchievementUnlock,
    ApiClientRequestTypeSendTracking,
    ApiClientRequestTypeCharge,
    ApiClientRequestTypeIsVisibleFriend,
    ApiClientRequestTypeServerList,
    ApiClientRequestTypeHealthCheckServer,
}ApiClientRequestType;
ApiClientRequestType apiClientRequiestTypeFromNSString(NSString *requestType);

@end
