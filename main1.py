import matplotlib
import numpy
from matplotlib import pyplot as plt
matplotlib.use('TkAgg')  # 或 'Qt5Agg'、'Agg'（无 GUI）
if __name__ == '__main__':
    u = numpy.random.uniform(0.0, 1.0, 10000)
    plt.hist(u, 80, facecolor='g', alpha=0.75)
    plt.grid(True)
    plt.show()

    times = 10000
    for time in range(times):
        u += numpy.random.uniform(0.0, 1.0, 10000)
    print (len(u))
    u /= times
    print (len(u))
    plt.hist(u, 80, facecolor='g', alpha=0.75)
    plt.grid(True)
    plt.show()