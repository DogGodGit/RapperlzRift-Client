//
//  LogUtil.h
//  MobbloKitApp
//
//  Created by galalab on 2015. 11. 12..
//  Copyright © 2015년 galalab. All rights reserved.
//

#ifndef LogUtil_h
#define LogUtil_h

#ifdef DEBUG

#define DNSLog(...)             NSLog(__VA_ARGS__)
#define DNSLogMethod()          NSLog(@"%s", __func__)
#define DNSLogArguments(...)    NSLog(@"%s - %@", __func__, [NSString stringWithFormat:__VA_ARGS__])
#define DNSLogView(v)           NSLog(@"%@", [v performSelector:@selector(recursiveDescription)])
#define DNSLogDebug(...)        NSLog(@" [DEBUG] %@", [NSString stringWithFormat:__VA_ARGS__])
#define DNSLogError(...)        NSLog(@" [ERROR] %@", [NSString stringWithFormat:__VA_ARGS__])

#else

#define DNSLog(...)             ;
#define DNSLogMethod()          ;
#define DNSLogArguments(...)    ;
#define DNSLogView(v)           ;
#define DNSLogDebug(...)        ;
#define DNSLogError(...)        NSLog(@" [ERROR] %@", [NSString stringWithFormat:__VA_ARGS__])

#endif

#endif /* LogUtil_h */
