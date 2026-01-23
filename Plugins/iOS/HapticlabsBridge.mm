#import <Foundation/Foundation.h>

#import "HapticlabsPlayer-Swift.h"

static HapticlabsPlayer *hapticlabsPlayer = nil;

typedef void (*CompletionCallback)(unsigned int id);
typedef void (*FailureCallback)(unsigned int id, const char *error);

extern "C" {

void _initializeIOSHapticlabsPlayer() {
  if (hapticlabsPlayer == nil) {
    hapticlabsPlayer = [[HapticlabsPlayer alloc] init];
  }
}

void _playPredefinedIOSVibration(const char *patternName) {
  _initializeIOSHapticlabsPlayer();

  NSString *patternNSString = [NSString stringWithUTF8String:patternName];

  [hapticlabsPlayer playPredefinedIOSVibration:patternNSString];
}

void _playIOSAHAP(const char *ahapPath, unsigned int id,
                  CompletionCallback onCompletion, FailureCallback onFailure) {
  _initializeIOSHapticlabsPlayer();

  NSString *ahapPathNSString = [NSString stringWithUTF8String:ahapPath];

  [hapticlabsPlayer playAHAPWithAhapPath:ahapPathNSString
      onCompletion:^{
        if (onCompletion != NULL)
          onCompletion(id);
      }
      onFailure:^(NSString *error) {
        if (onFailure != NULL) {
          const char *errorMsg = [error UTF8String];
          onFailure(id, errorMsg);
        }
      }];
}

void _setIOSAHAPHapticsMuted(bool muted) {
  _initializeIOSHapticlabsPlayer();

  [hapticlabsPlayer setHapticsMuteWithMute:muted];
}

bool _isIOSAHAPHapticsMuted() {
  _initializeIOSHapticlabsPlayer();

  bool isMuted = [hapticlabsPlayer isHapticsMuted];
  return isMuted;
}

void _setIOSAHAPAudioMuted(bool muted) {
  _initializeIOSHapticlabsPlayer();

  [hapticlabsPlayer setAudioMuteWithMute:muted];
}

bool _isIOSAHAPAudioMuted() {
  _initializeIOSHapticlabsPlayer();

  bool isMuted = [hapticlabsPlayer isAudioMuted];
  return isMuted;
}
}
