import React, { useEffect, useRef } from 'react';
import { Animated, Image, SafeAreaView, StyleSheet } from 'react-native';

export default function Splash({ navigation }) {
  const animationProgress = useRef(new Animated.Value(0)).current

  useEffect(() => {
    Animated.timing(animationProgress, {
      toValue: 5,
      duration: 5000,
      useNativeDriver: true
    }).start(() => {
      navigation.push('Login')
    })
  }, [])

  return (
    <SafeAreaView style={styles.container}>
      <Animated.View
        style={[
          styles.fadingContainer,
          {
            // Bind opacity to animated value
            opacity: animationProgress
          }
        ]}
      >
        <Image
          source={require('../assets/ForBov.png')}
          style={{ width: 300, height: 300 }}
        />
      </Animated.View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center"
  },
  fadingContainer: {
    padding: 20
  },
  fadingText: {
    fontSize: 28
  },
  buttonRow: {
    flexBasis: 100,
    justifyContent: "space-evenly",
    marginVertical: 16
  }
});
