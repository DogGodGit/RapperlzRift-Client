//
//  JsonUtil.h
//  Unity-iPhone
//
//  Created by galalab on 2016. 6. 17..
//
//

#import <Foundation/Foundation.h>

@interface JsonUtil : NSObject

+ (id)deserializeObjectFromJSON:(NSData *)jsonStringData;
+ (NSString *)serializeObjectToJSON:(id)object;
@end
