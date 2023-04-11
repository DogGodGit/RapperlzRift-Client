//
//  JsonUtil.m
//  Unity-iPhone
//
//  Created by galalab on 2016. 6. 17..
//
//

#import "JsonUtil.h"

@implementation JsonUtil

+ (id)deserializeObjectFromJSON:(NSData *)jsonStringData
{
    // iOS 5 NSJSONSerialization
    Class nsjson = NSClassFromString(@"NSJSONSerialization");
    if (nsjson) {
        NSError *e = nil;
        id object = [NSJSONSerialization JSONObjectWithData:jsonStringData options:0 error:&e];
        if (e) {
            NSLog(@"%@, jsonStringData = %@", e, [[NSString alloc] initWithData:jsonStringData encoding:NSUTF8StringEncoding]);
        }
        return object;
    }
    
    NSAssert(NO, @"No JSON serializers available!");
    return nil;
}

+ (NSString *)serializeObjectToJSON:(id)object
{
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:0 error:&err];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonString;
}

@end
